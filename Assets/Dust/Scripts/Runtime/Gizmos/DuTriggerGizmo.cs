﻿using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Trigger Gizmo")]
    [ExecuteInEditMode]
    public class DuTriggerGizmo : DuGizmoObject
    {
        public enum MessagePosition
        {
            Top,
            Bottom,
            Left,
            Right,
            Center,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private float m_Size = 1.0f;
        public float size
        {
            get => m_Size;
            set => m_Size = Normalizer.Size(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Color m_TriggeredColor = Color.red;
        public Color triggeredColor
        {
            get => m_TriggeredColor;
            set => m_TriggeredColor = value;
        }

        [SerializeField]
        private float m_TriggeredSize = 1.0f;
        public float triggeredSize
        {
            get => m_TriggeredSize;
            set => m_TriggeredSize = Normalizer.Size(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ShowMessage = false;
        public bool showMessage
        {
            get => m_ShowMessage;
            set => m_ShowMessage = value;
        }

        [SerializeField]
        private bool m_HideMessageOnIdleState = false;
        public bool hideMessageOnIdleState
        {
            get => m_HideMessageOnIdleState;
            set => m_HideMessageOnIdleState = value;
        }

        [SerializeField]
        private string m_Message = "Trigger Message";
        public string message
        {
            get => m_Message;
            set => m_Message = value;
        }

        [SerializeField]
        private MessagePosition m_MessagePosition = MessagePosition.Top;
        public MessagePosition messagePosition
        {
            get => m_MessagePosition;
            set => m_MessagePosition = value;
        }

        [SerializeField]
        private float m_MessageOffset = 0.25f;
        public float messageOffset
        {
            get => m_MessageOffset;
            set => m_MessageOffset = Normalizer.MessageOffset(value);
        }

        [SerializeField]
        private float m_MessageSize = 1.0f;
        public float messageSize
        {
            get => m_MessageSize;
            set => m_MessageSize = Normalizer.MessageSize(value);
        }

        [SerializeField]
        private bool m_MessageSizeInDepth = false;
        public bool messageSizeInDepth
        {
            get => m_MessageSizeInDepth;
            set => m_MessageSizeInDepth = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_FalloffDuration = 0.2f;
        public float falloffDuration
        {
            get => m_FalloffDuration;
            set => m_FalloffDuration = Normalizer.FalloffDuration(value);
        }

        [SerializeField]
        private Vector3 m_Center = Vector3.zero;
        public Vector3 center
        {
            get => m_Center;
            set => m_Center = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private float m_TriggerState;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Trigger")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Trigger Gizmo", typeof(DuTriggerGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            if (!Application.isPlaying)
            {
                EditorUpdateReset();

                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
        }

        void OnDisable()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.update -= EditorUpdate;
            }
        }

        void EditorUpdate()
        {
            float deltaTime;

            if (!EditorUpdateTick(out deltaTime))
                return;

            UpdateState(deltaTime);
        }

        void Update()
        {
            if (!Application.isPlaying)
                return;

            UpdateState(Time.deltaTime);
        }

        void UpdateState(float deltaTime)
        {
            m_TriggerState = Mathf.Clamp01(m_TriggerState - deltaTime / falloffDuration);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Trigger()
        {
            m_TriggerState = 1.0f;
        }

        public void Trigger(string newMessage)
        {
            m_TriggerState = 1.0f;
            m_Message = newMessage;
        }

        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.Lerp(color, triggeredColor, m_TriggerState);
            Gizmos.DrawSphere(center, 1.0f * Mathf.Lerp(size, triggeredSize, m_TriggerState));

            if (showMessage && !(hideMessageOnIdleState && DuMath.IsZero(m_TriggerState)))
            {
                Vector3 worldPosition = transform.TransformPoint(center);

                GUIStyle style = new GUIStyle("Label");
                style.fixedWidth = 1000;
                style.fixedHeight = 1000;

                if (messageSizeInDepth)
                    style.fontSize = Mathf.RoundToInt(style.fontSize * messageSize * 3f / HandleUtility.GetHandleSize(worldPosition));
                else
                    style.fontSize = Mathf.RoundToInt(style.fontSize * messageSize);

                if (hideMessageOnIdleState)
                    style.normal.textColor = Color.white * m_TriggerState;

                Vector3 offset = Vector3.zero;

                switch (messagePosition)
                {
                    case MessagePosition.Top:
                        offset = Camera.current.transform.up;
                        style.alignment = TextAnchor.LowerCenter;
                        break;

                    case MessagePosition.Bottom:
                        offset = -Camera.current.transform.up;
                        style.alignment = TextAnchor.UpperCenter;
                        break;

                    case MessagePosition.Left:
                        offset = -Camera.current.transform.right;
                        style.alignment = TextAnchor.MiddleRight;
                        break;

                    case MessagePosition.Right:
                        offset = Camera.current.transform.right;
                        style.alignment = TextAnchor.MiddleLeft;
                        break;

                    case MessagePosition.Center:
                        offset = Vector3.zero;
                        style.alignment = TextAnchor.MiddleCenter;
                        break;
                }

                Handles.Label(worldPosition + offset * (1f + messageOffset), message, style);
            }

            // DustGUI.ForcedRedrawSceneView();
            SceneView.lastActiveSceneView.Repaint();
        }

        void Reset()
        {
            color = Color.yellow;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static float Size(float value)
            {
                return Mathf.Abs(value);
            }

            public static float FalloffDuration(float value)
            {
                return Mathf.Clamp(value, 0.01f, float.MaxValue);
            }

            public static float MessageOffset(float value)
            {
                return Mathf.Clamp(value, 0f, +1f);
            }

            public static float MessageSize(float value)
            {
                return Mathf.Clamp(value, 0.01f, +1000f);
            }
        }
    }
}
#endif