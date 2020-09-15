using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Object Fields/Directional Field")]
    public class DuDirectionalField : DuObjectField
    {
        [SerializeField]
        private float m_Length = 1.0f;
        public float length
        {
            get => m_Length;
            set => m_Length = ShapeNormalizer.Length(value);
        }

        [SerializeField]
        private Axis6xDirection m_Direction = Axis6xDirection.XPlus;
        public Axis6xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        [SerializeField]
        private float m_GizmoWidth = 4.0f;
        public float gizmoWidth
        {
            get => m_GizmoWidth;
            set => m_GizmoWidth = ShapeNormalizer.GizmoWidth(value);
        }

        [SerializeField]
        private float m_GizmoHeight = 2.0f;
        public float gizmoHeight
        {
            get => m_GizmoHeight;
            set => m_GizmoHeight = ShapeNormalizer.GizmoHeight(value);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Object Fields/Directional")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuDirectionalField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string FieldName()
        {
            return "Directional";
        }

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            if (DuMath.IsZero(length))
                return remapping.MapValue(0f, timeSinceStart);

            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            float distanceToPoint;

            switch (direction)
            {
                default:
                case Axis6xDirection.XPlus:  distanceToPoint = -localPosition.x; break;
                case Axis6xDirection.XMinus: distanceToPoint = +localPosition.x; break;
                case Axis6xDirection.YPlus:  distanceToPoint = -localPosition.y; break;
                case Axis6xDirection.YMinus: distanceToPoint = +localPosition.y; break;
                case Axis6xDirection.ZPlus:  distanceToPoint = -localPosition.z; break;
                case Axis6xDirection.ZMinus: distanceToPoint = +localPosition.z; break;
            }

            float halfLength = length / 2f;

            float offset = 1f - DuMath.Map(-halfLength, +halfLength, 0f, 1f, distanceToPoint);

            return remapping.MapValue(offset, timeSinceStart);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            float halfLength = length / 2f;

            Vector3 plainSize = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(0.001f, gizmoHeight, gizmoWidth));
            Vector3 offsetPlane0 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-halfLength, 0f, 0f));
            Vector3 offsetPlane1 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(+halfLength, 0f, 0f));

            Gizmos.matrix = transform.localToWorldMatrix;

            Color colorRange0 = GetGizmoColorRange0();
            Color colorRange1 = GetGizmoColorRange1();

            if (remapping.enabled)
            {
                // End plane
                Gizmos.color = !remapping.invert ? colorRange1 : colorRange0;
                Gizmos.DrawWireCube(offsetPlane1, plainSize);

                // Middle plane
                Gizmos.DrawWireCube(DuVector3.Map01To(offsetPlane0, offsetPlane1, 1f - remapping.innerOffset), plainSize);

                // Begin plane
                Gizmos.color = !remapping.invert ? colorRange0 : colorRange1;
                Gizmos.DrawWireCube(offsetPlane0, plainSize);
            }
            else
            {
                // End plane
                Gizmos.color = colorRange0;
                Gizmos.DrawWireCube(offsetPlane0, plainSize);

                // Begin plane
                Gizmos.color = colorRange1;
                Gizmos.DrawWireCube(offsetPlane1, plainSize);
            }

            // 3: Draw arrow
            float arrowSign = remapping.enabled && remapping.invert ? -1f : +1f;
            Gizmos.color = colorRange1;
            Gizmos.DrawRay(offsetPlane0 * arrowSign, (offsetPlane1 - offsetPlane0) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, 0f, +0.06f) * halfLength) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, 0f, -0.06f) * halfLength) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, +0.06f, 0f) * halfLength) * arrowSign);
            Gizmos.DrawRay(offsetPlane1 * arrowSign, DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, new Vector3(-0.2f, -0.06f, 0f) * halfLength) * arrowSign);
        }

        private void Reset()
        {
            ResetDefaults();

            remapping.innerOffset = 0f;
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        internal static class ShapeNormalizer
        {
            public static float Length(float value)
            {
                return Mathf.Max(0f, value);
            }

            public static float GizmoWidth(float value)
            {
                return Mathf.Abs(value);
            }

            public static float GizmoHeight(float value)
            {
                return Mathf.Abs(value);
            }
        }
    }
}
