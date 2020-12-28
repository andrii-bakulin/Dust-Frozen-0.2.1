using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCapture))]
    [CanEditMultipleObjects]
    public class DuCaptureEditor : DuEditor
    {
        public enum Resolution
        {
            Custom = 0,

            Landscape1280x720 = 101,
            Landscape1920x1080 = 102,
            Landscape3840x2160 = 103,
            Landscape1024x768 = 105,
            Landscape2048x1536 = 104,

            Portrait720x1280 = 201,
            Portrait1080x1920 = 202,
            Portrait2160x3840 = 203,
            Portrait768x1024 = 205,
            Portrait1536x2048 = 204,

            Square512x512 = 301,
            Square1024x1024 = 302,
            Square2048x2048 = 303,
            Square4096x4096 = 304,
            Square8192x8192 = 305,
        }

        private DuProperty m_Width;
        private DuProperty m_Height;
        private DuProperty m_FrameRate;

        private DuProperty m_BaseFolder;
        private DuProperty m_FilePrefix;
        private DuProperty m_FileFormat;
        private DuProperty m_Quality;

        private DuProperty m_FrameRange;
        private DuProperty m_SkipFrames;
        private DuProperty m_RecordFrames;
        private DuProperty m_AutoRecording;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Utilities/Capture")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedObjects(typeof(DuCapture));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_Width = FindProperty("m_Width", "Width");
            m_Height = FindProperty("m_Height", "Height");
            m_FrameRate = FindProperty("m_FrameRate", "Frame Rate");

            m_BaseFolder = FindProperty("m_BaseFolder", "Base Folder");
            m_FilePrefix = FindProperty("m_FilePrefix", "File Prefix");
            m_FileFormat = FindProperty("m_FileFormat", "Format");
            m_Quality = FindProperty("m_Quality", "Quality");

            m_FrameRange = FindProperty("m_FrameRange", "Frame Range");
            m_SkipFrames = FindProperty("m_SkipFrames", "Skip Frames");
            m_RecordFrames = FindProperty("m_RecordFrames", "Record Frames");
            m_AutoRecording = FindProperty("m_AutoRecording", "Auto Recording");
        }

        public override void OnInspectorGUI()
        {
            var main = target as DuCapture;

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Quality", "DuCapture.Quality"))
            {
                Resolution resolutionCur, resolutionNew;

                switch (m_Width.valInt + "x" + m_Height.valInt)
                {
                    case "1280x720":  resolutionCur = Resolution.Landscape1280x720;  break;
                    case "1920x1080": resolutionCur = Resolution.Landscape1920x1080; break;
                    case "3840x2160": resolutionCur = Resolution.Landscape3840x2160; break;
                    case "1024x768":  resolutionCur = Resolution.Landscape1024x768;  break;
                    case "2048x1536": resolutionCur = Resolution.Landscape2048x1536; break;

                    case "720x1280":  resolutionCur = Resolution.Portrait720x1280;  break;
                    case "1080x1920": resolutionCur = Resolution.Portrait1080x1920; break;
                    case "2160x3840": resolutionCur = Resolution.Portrait2160x3840; break;
                    case "768x1024":  resolutionCur = Resolution.Portrait768x1024;  break;
                    case "1536x2048": resolutionCur = Resolution.Portrait1536x2048; break;

                    case "512x512":   resolutionCur = Resolution.Square512x512;   break;
                    case "1024x1024": resolutionCur = Resolution.Square1024x1024; break;
                    case "2048x2048": resolutionCur = Resolution.Square2048x2048; break;
                    case "4096x4096": resolutionCur = Resolution.Square4096x4096; break;
                    case "8192x8192": resolutionCur = Resolution.Square8192x8192; break;

                    default: resolutionCur = Resolution.Custom; break;
                }

                resolutionNew = (Resolution) DustGUI.DropDownList("Resolution", resolutionCur);

                if (resolutionNew != resolutionCur)
                {
                    switch (resolutionNew)
                    {
                        default:
                        case Resolution.Custom:             m_Width.valInt =    1; m_Height.valInt =    1; break;

                        case Resolution.Landscape1280x720:  m_Width.valInt = 1280; m_Height.valInt =  720; break;
                        case Resolution.Landscape1920x1080: m_Width.valInt = 1920; m_Height.valInt = 1080; break;
                        case Resolution.Landscape3840x2160: m_Width.valInt = 3840; m_Height.valInt = 2160; break;
                        case Resolution.Landscape1024x768:  m_Width.valInt = 1024; m_Height.valInt =  768; break;
                        case Resolution.Landscape2048x1536: m_Width.valInt = 2048; m_Height.valInt = 1536; break;

                        case Resolution.Portrait720x1280:   m_Width.valInt =  720; m_Height.valInt = 1280; break;
                        case Resolution.Portrait1080x1920:  m_Width.valInt = 1080; m_Height.valInt = 1920; break;
                        case Resolution.Portrait2160x3840:  m_Width.valInt = 2160; m_Height.valInt = 3840; break;
                        case Resolution.Portrait768x1024:   m_Width.valInt =  768; m_Height.valInt = 1024; break;
                        case Resolution.Portrait1536x2048:  m_Width.valInt = 1536; m_Height.valInt = 2048; break;

                        case Resolution.Square512x512:      m_Width.valInt =  512; m_Height.valInt =  512; break;
                        case Resolution.Square1024x1024:    m_Width.valInt = 1024; m_Height.valInt = 1024; break;
                        case Resolution.Square2048x2048:    m_Width.valInt = 2048; m_Height.valInt = 2048; break;
                        case Resolution.Square4096x4096:    m_Width.valInt = 4096; m_Height.valInt = 4096; break;
                        case Resolution.Square8192x8192:    m_Width.valInt = 8192; m_Height.valInt = 8192; break;
                    }
                }

                PropertyField(m_Width);
                PropertyField(m_Height);
                PropertyField(m_FrameRate);
            }
            DustGUI.FoldoutEnd();

            if (DustGUI.FoldoutBegin("Location", "DuCapture.Location"))
            {
                PropertyField(m_BaseFolder);
                PropertyField(m_FilePrefix);
                PropertyField(m_FileFormat);

                if ((DuCapture.Format)m_FileFormat.enumValueIndex == DuCapture.Format.JPG)
                {
                    PropertyExtendedIntSlider(m_Quality, 1, 100, 1, 1, 100);
                }

                Space();

                int frameIndex = main.isRecording ? main.frameIndex : 12345;

                string fullPath = main.GetFullPath();
                string fullFileName = main.GetFullFilename(fullPath, m_FilePrefix.valString, frameIndex, (DuCapture.Format) m_FileFormat.enumValueIndex);
                DustGUI.HelpBoxInfo("PREVIEW:" + "\n" + fullFileName);
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Recording", "DuCapture.Recording"))
            {
                PropertyField(m_FrameRange);

                var frameRange = (DuCapture.FrameRange) m_FrameRange.enumValueIndex;

                switch (frameRange)
                {
                    case DuCapture.FrameRange.Limited:
                        PropertyField(m_SkipFrames);
                        PropertyField(m_RecordFrames);
                        DustGUI.StaticTextField("Record Duration", DuMath.Round((float)m_RecordFrames.valInt / m_FrameRate.valInt, 2) + " sec");
                        break;

                    case DuCapture.FrameRange.Unlimited:
                        break;
                }

                PropertyField(m_AutoRecording);

                DustGUI.Space();
                DustGUI.BeginHorizontal();

                string progressBarTitle = "-";
                float progressBarState = 0.0f;

                if (EditorApplication.isPlaying)
                {
                    if (main.isRecording)
                    {
                        if (DustGUI.Button("Stop Recording"))
                            main.StopRecording();

                        switch (frameRange)
                        {
                            case DuCapture.FrameRange.Limited:
                                if (main.skipFrames > 0 && main.frameIndex < main.skipFrames)
                                {
                                    progressBarState = 1f - (float)main.frameIndex / main.skipFrames;
                                    progressBarTitle = string.Format("Skip {0} frames", main.skipFrames - main.frameIndex);
                                }
                                else
                                {
                                    progressBarState = (float)(main.frameIndex - main.skipFrames) / main.recordFrames;
                                    progressBarTitle = string.Format("Recorded {0} frames", main.frameIndex);
                                }
                                break;

                            case DuCapture.FrameRange.Unlimited:
                                progressBarState = Mathf.PingPong(main.timeSinceStartRecording/3f, 1f);
                                progressBarTitle = string.Format("Recorded {0} frames", main.frameIndex);
                                break;
                        }
                    }
                    else
                    {
                        if (DustGUI.Button("Start Recording"))
                            main.StartRecording();

                        progressBarTitle = "Idle";
                    }

                    if (DustGUI.Button("Stop", 80, 30))
                        EditorApplication.isPlaying = false;
                }
                else
                {
                    if (DustGUI.Button(m_AutoRecording.IsTrue ? "Play + Record" : "Play"))
                        EditorApplication.isPlaying = true;
                }
                DustGUI.EndHorizontal();

                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                EditorGUI.ProgressBar(rect, progressBarState, progressBarTitle);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_Width.isChanged)
                m_Width.valInt = DuCapture.Normalizer.WidthAndHeight(m_Width.valInt);

            if (m_Height.isChanged)
                m_Height.valInt = DuCapture.Normalizer.WidthAndHeight(m_Height.valInt);

            if (m_FrameRate.isChanged)
                m_FrameRate.valInt = DuCapture.Normalizer.FrameRate(m_FrameRate.valInt);

            if (m_Quality.isChanged)
                m_Quality.valInt = DuCapture.Normalizer.Quality(m_Quality.valInt);

            if (m_SkipFrames.isChanged)
                m_SkipFrames.valInt = DuCapture.Normalizer.SkipFrames(m_SkipFrames.valInt);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();

            if (main.isRecording)
                DustGUI.ForcedRedrawInspector(this);
        }
    }
}
