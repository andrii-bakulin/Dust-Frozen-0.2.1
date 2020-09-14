using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Object Fields/Cube Field")]
    public class DuCubeField : DuObjectField
    {
        private class Calc
        {
            public Ray ray;
            public Plane planeX;
            public Plane planeY;
            public Plane planeZ;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_Size = Vector3.one;
        public Vector3 size
        {
            get => m_Size;
            set
            {
                m_Size = ShapeNormalizer.Size(value);
                ResetCalcData();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Calc m_Calc = null;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Object Fields/Cube")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuCubeField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string FieldName()
        {
            return "Cube";
        }

        public override float GetWeightForFieldPoint(DuField.Point fieldPoint)
        {
            InitCalcData();

            if (DuMath.IsZero(size.x) || DuMath.IsZero(size.y) || DuMath.IsZero(size.z))
                return remapping.MapValue(0f, timeSinceStart);

            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            float distanceToPoint = localPosition.magnitude;
            float distanceToEdge = Mathf.Infinity;

            m_Calc.ray.direction = DuVector3.Abs(localPosition);

            float distanceToPlane;

            if (m_Calc.planeX.Raycast(m_Calc.ray, out distanceToPlane) && distanceToPlane > 0)
                distanceToEdge = Mathf.Min(distanceToEdge, distanceToPlane);

            if (m_Calc.planeY.Raycast(m_Calc.ray, out distanceToPlane) && distanceToPlane > 0)
                distanceToEdge = Mathf.Min(distanceToEdge, distanceToPlane);

            if (m_Calc.planeZ.Raycast(m_Calc.ray, out distanceToPlane) && distanceToPlane > 0)
                distanceToEdge = Mathf.Min(distanceToEdge, distanceToPlane);

            if (DuMath.IsZero(distanceToEdge))
                return remapping.MapValue(0f, timeSinceStart); // but this never should execute!

            float offset = 1f - distanceToPoint / distanceToEdge;
            return remapping.MapValue(offset, timeSinceStart);
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void InitCalcData()
        {
            if (Dust.IsNotNull(m_Calc))
                return;

            Vector3 halfSize = size / 2f;

            m_Calc = new Calc();

            m_Calc.ray.origin = Vector3.zero;
            m_Calc.planeX.Set3Points(new Vector3(halfSize.x, 0, 0), new Vector3(halfSize.x, halfSize.y, 0), new Vector3(halfSize.x, halfSize.y, halfSize.z));
            m_Calc.planeY.Set3Points(new Vector3(0, halfSize.y, 0), new Vector3(0, halfSize.y, halfSize.z), new Vector3(halfSize.x, halfSize.y, halfSize.z));
            m_Calc.planeZ.Set3Points(new Vector3(0, 0, halfSize.z), new Vector3(halfSize.x, 0, halfSize.z), new Vector3(halfSize.x, halfSize.y, halfSize.z));
        }

        internal void ResetCalcData()
        {
            m_Calc = null;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = !remapping.invert ? k_GizmosColorRangeOne : k_GizmosColorRangeZero;
            Gizmos.DrawWireCube(Vector3.zero, size * remapping.innerOffset);

            Gizmos.color = !remapping.invert ? k_GizmosColorRangeZero : k_GizmosColorRangeOne;
            Gizmos.DrawWireCube(Vector3.zero, size);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        internal static class ShapeNormalizer
        {
            public static Vector3 Size(Vector3 value)
            {
                return DuVector3.Abs(value);
            }
        }
    }
}
