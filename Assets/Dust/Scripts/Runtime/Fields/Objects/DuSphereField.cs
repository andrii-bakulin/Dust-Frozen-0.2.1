﻿using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Object Fields/Sphere Field")]
    public class DuSphereField : DuObjectField
    {
        [SerializeField]
        private float m_Radius = 1.0f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = ShapeNormalizer.Radius(value);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Object Fields/Sphere")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuSphereField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, radius);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FieldName()
        {
            return "Sphere";
        }

#if UNITY_EDITOR
        public override string FieldEditorDynamicHint()
        {
            return "";
        }
#endif

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            if (DuMath.IsZero(radius))
                return remapping.MapValue(0f);

            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            float distanceToPoint = localPosition.magnitude;
            float distanceToEdge = radius;

            float offset = 1f - distanceToPoint / distanceToEdge;

            return remapping.MapValue(offset);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Color colorRange0 = GetGizmoColorRange0();
            Color colorRange1 = GetGizmoColorRange1();

            if (remapping.remapForceEnabled)
            {
                Gizmos.color = !remapping.invert ? colorRange1 : colorRange0;
                Gizmos.DrawWireSphere(Vector3.zero, radius * remapping.innerOffset);

                Gizmos.color = !remapping.invert ? colorRange0 : colorRange1;
                Gizmos.DrawWireSphere(Vector3.zero, radius);
            }
            else
            {
                Gizmos.color = colorRange0;
                Gizmos.DrawWireSphere(Vector3.zero, radius);
            }
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class ShapeNormalizer
        {
            public static float Radius(float value)
            {
                return Mathf.Abs(value);
            }
        }
    }
}
