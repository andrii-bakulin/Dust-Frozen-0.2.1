﻿using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Arrow Gizmo")]
    public class ArrowGizmo : GizmoObject
    {
        public enum AxisColorMode
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
        private AxisColorMode m_AxisColorMode = AxisColorMode.AutoSetByDirection;
        public AxisColorMode axisColorMode
        {
            get => m_AxisColorMode;
            set => m_AxisColorMode = value;
        }

        [SerializeField]
        private bool m_ShowStartPoint = true;
        public bool showStartPoint
        {
            get => m_ShowStartPoint;
            set => m_ShowStartPoint = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string GizmoName()
        {
            return "Arrow";
        }

#if UNITY_EDITOR
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

            switch (axisColorMode)
            {
                case AxisColorMode.AutoSetByDirection:
                    Handles.color = direction.normalized.duToAbs().duToColor();
                    break;

                case AxisColorMode.AxisX:
                    Handles.color = Handles.xAxisColor;
                    break;

                case AxisColorMode.AxisY:
                    Handles.color = Handles.yAxisColor;
                    break;

                case AxisColorMode.AxisZ:
                    Handles.color = Handles.zAxisColor;
                    break;

                case AxisColorMode.Custom:
                    Handles.color = color;
                    break;

                default:
                    break;
            }

            Handles.ArrowHandleCap(0,
                transform.localToWorldMatrix.MultiplyPoint(startPosition),
                transform.rotation * Quaternion.LookRotation(direction),
                size,
                EventType.Repaint);
        }
#endif
    }
}
