﻿using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Object Fields/Cone Field")]
    public class DuConeField : DuObjectField
    {
        [SerializeField]
        private float m_Radius = 1.0f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = ShapeNormalizer.Radius(value);
        }

        [SerializeField]
        private float m_Height = 2.0f;
        public float height
        {
            get => m_Height;
            set => m_Height = ShapeNormalizer.Height(value);
        }

        [SerializeField]
        private Axis6xDirection m_Direction = Axis6xDirection.YPlus;
        public Axis6xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Object Fields/Cone")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuConeField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string FieldName()
        {
            return "Cone";
        }

        public override float GetWeightForFieldPoint(DuField.Point fieldPoint)
        {
            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            // Convert to [X+]-axis-space by direction
            localPosition = DuAxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            float distanceToPoint = localPosition.magnitude;
            float distanceToEdge = DuMath.Cone.DistanceToEdge(radius, height, localPosition);

            float offset = distanceToEdge > 0f ? 1f - distanceToPoint / distanceToEdge : 0f;

            return remapping.MapValue(offset, timeSinceStart);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            float innerScale = remapping.innerOffset;

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = !remapping.invert ? k_GizmosColorRangeOne : k_GizmosColorRangeZero;
            DuGizmos.DrawWireCone(radius * innerScale, height * innerScale, Vector3.zero, direction, 32, 4);

            Gizmos.color = !remapping.invert ? k_GizmosColorRangeZero : k_GizmosColorRangeOne;
            DuGizmos.DrawWireCone(radius, height, Vector3.zero, direction, 32, 4);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        internal static class ShapeNormalizer
        {
            public static float Height(float value)
            {
                return Mathf.Abs(value);
            }

            public static float Radius(float value)
            {
                return Mathf.Abs(value);
            }
        }
    }
}
