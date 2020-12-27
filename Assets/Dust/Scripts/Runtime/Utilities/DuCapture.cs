using System;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Utilities/Capture")]
    [RequireComponent(typeof(Camera))]
    public class DuCapture : DuMonoBehaviour
    {
#if UNITY_EDITOR
        public enum Format
        {
            JPG = 0,
            PNG = 1,
            TGA = 2,
            EXR = 3,
            RAW = 4,
        };

        public enum FrameRange
        {
            Limited = 0,
            Unlimited = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private int m_Width = 1920;
        public int width
        {
            get => m_Width;
            set => m_Width = Normalizer.WidthAndHeight(value);
        }

        [SerializeField]
        private int m_Height = 1080;
        public int height
        {
            get => m_Height;
            set => m_Height = Normalizer.WidthAndHeight(value);
        }

        [SerializeField]
        private int m_FrameRate = 30;
        public int frameRate
        {
            get => m_FrameRate;
            set => m_FrameRate = Normalizer.FrameRate(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private string m_BaseFolder = "DustCapture";
        public string baseFolder
        {
            get => m_BaseFolder;
            set => m_BaseFolder = value;
        }

        [SerializeField]
        private string m_FilePrefix = "frame_";
        public string filePrefix
        {
            get => m_FilePrefix;
            set => m_FilePrefix = value;
        }

        [SerializeField]
        private Format m_FileFormat = Format.JPG;
        public Format fileFormat
        {
            get => m_FileFormat;
            set => m_FileFormat = value;
        }

        [SerializeField]
        private int m_Quality = 75;
        public int quality
        {
            get => m_Quality;
            set => m_Quality = Normalizer.Quality(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private FrameRange m_FrameRange = FrameRange.Limited;
        public FrameRange frameRange
        {
            get => m_FrameRange;
            set => m_FrameRange = value;
        }

        [SerializeField]
        private int m_SkipFrames = Normalizer.SkipFrames(0);
        public int skipFrames
        {
            get => m_SkipFrames;
            set => m_SkipFrames = value;
        }

        [SerializeField]
        private int m_RecordFrames = 900;
        public int recordFrames
        {
            get => m_RecordFrames;
            set => m_RecordFrames = value;
        }

        [SerializeField]
        private bool m_AutoRecording = false;
        public bool autoRecording
        {
            get => m_AutoRecording;
            set => m_AutoRecording = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private float m_TimeIdle;

        private int m_FrameIndex;
        public int frameIndex => m_FrameIndex;

        private float m_TimeSinceStartRecording;
        public float timeSinceStartRecording => m_TimeSinceStartRecording;

        private DateTime m_DateTime;
        private Rect m_CaptureRect;
        private RenderTexture m_RenderTexture;
        private Texture2D m_CaptureTexture;
        private string m_CaptureFolder;

        private bool m_IsRecording;
        public bool isRecording => m_IsRecording;

        //--------------------------------------------------------------------------------------------------------------

        void Awake()
        {
            if (autoRecording)
                StartRecording();
        }

        public void StartRecording()
        {
            if (m_IsRecording)
                return;

            if (width <= 0 || height <= 0)
            {
                Dust.Debug.Error("DuCapture: width and height values should be greater then 0");
                return;
            }

            m_IsRecording = true;

            m_TimeIdle = 0f;
            m_FrameIndex = 0;
            m_TimeSinceStartRecording = 0f;

            m_DateTime = DateTime.Now;
            m_CaptureRect = new Rect(0, 0, width, height);
            m_RenderTexture = new RenderTexture(width, height, 24);
            m_CaptureTexture = new Texture2D(width, height, TextureFormat.RGB24, false);

            m_CaptureFolder = GetFullPath();

            if (!System.IO.Directory.Exists(m_CaptureFolder))
                System.IO.Directory.CreateDirectory(m_CaptureFolder);
        }

        public void StopRecording()
        {
            if (!m_IsRecording)
                return;

            m_IsRecording = false;

            m_CaptureRect = Rect.zero;
            m_RenderTexture = null;
            m_CaptureTexture = null;
            m_CaptureFolder = null;
        }

        void LateUpdate()
        {
            if (!isRecording)
                return;

            m_TimeIdle -= Time.deltaTime;

            if (m_TimeIdle <= 0.0f)
            {
                m_TimeIdle = 1f / frameRate;

                switch (m_FrameRange)
                {
                    case FrameRange.Limited:
                        // Notice: If logic will change here, then require update logic when show progress bar
                        int lastFrame = skipFrames + recordFrames;

                        if (skipFrames <= frameIndex && frameIndex < lastFrame)
                            Capture();

                        if (frameIndex == lastFrame)
                            StopRecording();
                        break;

                    case FrameRange.Unlimited:
                        Capture();
                        break;
                }

                m_FrameIndex++;
            }

            m_TimeSinceStartRecording += Time.deltaTime;
        }

        void Capture()
        {
            if (Dust.IsNull(m_RenderTexture) || Dust.IsNull(m_CaptureTexture))
                return;

            // Render
            Camera cam = GetComponent<Camera>();

            cam.targetTexture = m_RenderTexture;
            cam.Render();

            RenderTexture.active = m_RenderTexture;
            m_CaptureTexture.ReadPixels(m_CaptureRect, 0, 0);
            RenderTexture.active = null;

            cam.targetTexture = null;

            // Save
            byte[] fileData;

            switch (fileFormat)
            {
                case Format.JPG: fileData = m_CaptureTexture.EncodeToJPG(quality); break;
                case Format.PNG: fileData = m_CaptureTexture.EncodeToPNG(); break;
                case Format.TGA: fileData = m_CaptureTexture.EncodeToTGA(); break;
                case Format.EXR: fileData = m_CaptureTexture.EncodeToEXR(); break;
                case Format.RAW: fileData = m_CaptureTexture.GetRawTextureData(); break;
                default:
                    Debug.LogError("Undefined file format");
                    return;
            }

            string filename = GetFullFilename(m_CaptureFolder, m_FilePrefix, m_FrameIndex, m_FileFormat);

            new System.Threading.Thread(() =>
            {
                var f = System.IO.File.Create(filename);
                f.Write(fileData, 0, fileData.Length);
                f.Close();
            }).Start();
        }

        public string GetFullPath()
        {
            string subFolder = m_IsRecording ? "REC-" + m_DateTime.ToString("yyyyMMdd-HHmmss") : "REC-YYYYMMDD-HHMMSS";
            return (!baseFolder.Equals("") ? baseFolder : "DustCapture") + "/" + subFolder;
        }

        public string GetFullFilename(string folder, string prefix, int frameIndex, Format format)
        {
            string filename = prefix + frameIndex.ToString().PadLeft(5, '0') + "." + format.ToString().ToLower();
            return string.Format("{0}/{1}", folder, filename);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static int WidthAndHeight(int value)
            {
                return Mathf.Clamp(value, 1, 8192);
            }

            public static int FrameRate(int value)
            {
                return Mathf.Clamp(value, 1, 60);
            }

            public static int Quality(int value)
            {
                return Mathf.Clamp(value, 1, 100);
            }

            public static int SkipFrames(int value)
            {
                if (value < 0)
                    value = 0;
                return value;
            }
        }
#endif
    }
}
