using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Arrow Gizmo")]
    public class DuArrowGizmo : DuGizmoObject
    {
        public enum ColorMode
        {
            AutoSetByDirection = 0,
            AxisX = 1,
            AxisY = 2,
            AxisZ = 3,
            Custom = 4,
        }

        [SerializeField]
        private Vector3 m_Direction = Vector3.forward;
        public Vector3 direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        [SerializeField]
        private Vector3 m_StartPosition = Vector3.zero;
        public Vector3 startPosition
        {
            get => m_StartPosition;
            set => m_StartPosition = value;
        }

        [SerializeField]
        private float m_Size = 1.5f;
        public float size
        {
            get => m_Size;
            set => m_Size = value;
        }

        [SerializeField]
        private ColorMode m_ColorMode = ColorMode.AutoSetByDirection;
        public ColorMode colorMode
        {
            get => m_ColorMode;
            set => m_ColorMode = value;
        }

        [SerializeField]
        private bool m_ShowStartPoint = true;
        public bool showStartPoint
        {
            get => m_ShowStartPoint;
            set => m_ShowStartPoint = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Arrow")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Arrow Gizmo", typeof(DuArrowGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void DrawGizmos()
        {
            if (showStartPoint)
            {
                Gizmos.color = Color.yellow;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawSphere(startPosition, 0.05f * size);
            }

            if (direction.Equals(Vector3.zero))
                return;

            switch (colorMode)
            {
                case ColorMode.AutoSetByDirection:
                    Handles.color = direction.normalized.abs().ToColor();
                    break;

                case ColorMode.AxisX:
                    Handles.color = Handles.xAxisColor;
                    break;

                case ColorMode.AxisY:
                    Handles.color = Handles.yAxisColor;
                    break;

                case ColorMode.AxisZ:
                    Handles.color = Handles.zAxisColor;
                    break;

                case ColorMode.Custom:
                    Handles.color = color;
                    break;
            }

            Handles.ArrowHandleCap(0,
                transform.localToWorldMatrix.MultiplyPoint(startPosition),
                transform.rotation * Quaternion.LookRotation(direction),
                size,
                EventType.Repaint);
        }
    }
}
#endif
