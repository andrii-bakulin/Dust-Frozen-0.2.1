using UnityEditor;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Deformers/Twist Deformer")]
    public class DuTwistDeformer : DuDeformer
    {
        public enum ImpactMode
        {
            Limited = 0,
            Unlimited = 1,
            WithinBox = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ImpactMode m_ImpactMode = ImpactMode.Limited;
        public ImpactMode impactMode
        {
            get => m_ImpactMode;
            set => m_ImpactMode = value;
        }

        [SerializeField]
        private Vector3 m_Size = Vector3.one * 2f;
        public Vector3 size
        {
            get => m_Size;
            set => m_Size = Normalizer.Size(value);
        }

        [SerializeField]
        private float m_Angle = 0f;
        public float angle
        {
            get => m_Angle;
            set => m_Angle = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Deformers/Twist")]
        public static void AddComponent()
        {
            AddDeformerComponentByType(typeof(DuTwistDeformer));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string DeformerName()
        {
            return "Twist";
        }

        public override bool DeformPoint(ref Vector3 localPosition, float strength = 1f)
        {
            float halfSizeY = size.y / 2f;
            float weight;

            if (impactMode == ImpactMode.WithinBox && !IsPointInsideDeformBox(localPosition, size))
                return false;

            switch (impactMode)
            {
                default:
                case ImpactMode.Limited:
                case ImpactMode.WithinBox:
                    weight = DuMath.Map(-halfSizeY, +halfSizeY, 0f, 1f, localPosition.y, true);
                    break;

                case ImpactMode.Unlimited:
                    weight = DuMath.Map(-halfSizeY, +halfSizeY, 0f, 1f, localPosition.y);
                    break;
            }

            DuMath.RotatePoint(ref localPosition.x, ref localPosition.z, weight * angle * strength);
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawDeformerGizmos()
        {
            Vector3 halfSize = size / 2f;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = k_GizmosColorMain;

            // Bottom
            OnDrawGizmosLine(new Vector3(+halfSize.x, -halfSize.y, +halfSize.z), new Vector3(+halfSize.x, -halfSize.y, -halfSize.z));
            OnDrawGizmosLine(new Vector3(+halfSize.x, -halfSize.y, -halfSize.z), new Vector3(-halfSize.x, -halfSize.y, -halfSize.z));
            OnDrawGizmosLine(new Vector3(-halfSize.x, -halfSize.y, -halfSize.z), new Vector3(-halfSize.x, -halfSize.y, +halfSize.z));
            OnDrawGizmosLine(new Vector3(-halfSize.x, -halfSize.y, +halfSize.z), new Vector3(+halfSize.x, -halfSize.y, +halfSize.z));

            // Top
            OnDrawGizmosLine(new Vector3(+halfSize.x, +halfSize.y, +halfSize.z), new Vector3(+halfSize.x, +halfSize.y, -halfSize.z));
            OnDrawGizmosLine(new Vector3(+halfSize.x, +halfSize.y, -halfSize.z), new Vector3(-halfSize.x, +halfSize.y, -halfSize.z));
            OnDrawGizmosLine(new Vector3(-halfSize.x, +halfSize.y, -halfSize.z), new Vector3(-halfSize.x, +halfSize.y, +halfSize.z));
            OnDrawGizmosLine(new Vector3(-halfSize.x, +halfSize.y, +halfSize.z), new Vector3(+halfSize.x, +halfSize.y, +halfSize.z));

            // Side lines
            int steps = Mathf.Abs(Mathf.FloorToInt(angle / 36)) + 10;

            for (int i = 0; i < steps; i++)
            {
                float y0 = Mathf.Lerp(-halfSize.y, +halfSize.y, (float) (i + 0) / steps);
                float y1 = Mathf.Lerp(-halfSize.y, +halfSize.y, (float) (i + 1) / steps);

                OnDrawGizmosLine(new Vector3(+halfSize.x, y0, +halfSize.z), new Vector3(+halfSize.x, y1, +halfSize.z));
                OnDrawGizmosLine(new Vector3(+halfSize.x, y0, -halfSize.z), new Vector3(+halfSize.x, y1, -halfSize.z));
                OnDrawGizmosLine(new Vector3(-halfSize.x, y0, +halfSize.z), new Vector3(-halfSize.x, y1, +halfSize.z));
                OnDrawGizmosLine(new Vector3(-halfSize.x, y0, -halfSize.z), new Vector3(-halfSize.x, y1, -halfSize.z));
            }
        }

        private void OnDrawGizmosLine(Vector3 point0, Vector3 point1)
        {
            DeformPoint(ref point0);
            DeformPoint(ref point1);
            Gizmos.DrawLine(point0, point1);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static Vector3 Size(Vector3 value)
            {
                return DuVector3.Abs(value);
            }
        }
    }
}
