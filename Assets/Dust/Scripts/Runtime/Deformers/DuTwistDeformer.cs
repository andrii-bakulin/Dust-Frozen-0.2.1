using UnityEditor;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Deformers/Twist Deformer")]
    public class DuTwistDeformer : DuDeformer
    {
        public enum DeformMode
        {
            Limited = 0,
            Unlimited = 1,
            WithinBox = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private DeformMode m_DeformMode = DeformMode.Limited;
        public DeformMode deformMode
        {
            get => m_DeformMode;
            set => m_DeformMode = value;
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

        [SerializeField]
        private Axis6xDirection m_Direction = Axis6xDirection.YPlus;
        public Axis6xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
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

#if UNITY_EDITOR
        public override string DeformerDynamicHint()
        {
            return angle.ToString("F2") + "° in " + DuAxisDirection.ToString(direction);
        }
#endif

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool DeformPoint(ref Vector3 localPosition, float strength = 1f)
        {
            if (deformMode == DeformMode.WithinBox && !IsPointInsideDeformBox(localPosition, size))
                return false;

            var xpAxisPosition = DuAxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Deform logic (in X+ direction)

            float twistHalfSize = size.x / 2f;
            float deformPower;

            switch (deformMode)
            {
                default:
                case DeformMode.Limited:
                case DeformMode.WithinBox:
                    deformPower = DuMath.Fit(-twistHalfSize, +twistHalfSize, 0f, 1f, xpAxisPosition.x, true);
                    break;

                case DeformMode.Unlimited:
                    deformPower = DuMath.Fit(-twistHalfSize, +twistHalfSize, 0f, 1f, xpAxisPosition.x);
                    break;
            }

            DuMath.RotatePoint(ref xpAxisPosition.y, ref xpAxisPosition.z, deformPower * angle * strength);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            localPosition = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, xpAxisPosition);
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, deformMode);
            DuDynamicState.Append(ref dynamicState, ++seq, size);
            DuDynamicState.Append(ref dynamicState, ++seq, angle);
            DuDynamicState.Append(ref dynamicState, ++seq, direction);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawDeformerGizmos()
        {
            Vector3 halfSize = size / 2f;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = enabled ? k_GizmosColorActive : k_GizmosColorDisabled;

            // Bottom
            DrawGizmosLine(new Vector3(+halfSize.x, -halfSize.y, +halfSize.z), new Vector3(+halfSize.x, -halfSize.y, -halfSize.z));
            DrawGizmosLine(new Vector3(+halfSize.x, -halfSize.y, -halfSize.z), new Vector3(-halfSize.x, -halfSize.y, -halfSize.z));
            DrawGizmosLine(new Vector3(-halfSize.x, -halfSize.y, -halfSize.z), new Vector3(-halfSize.x, -halfSize.y, +halfSize.z));
            DrawGizmosLine(new Vector3(-halfSize.x, -halfSize.y, +halfSize.z), new Vector3(+halfSize.x, -halfSize.y, +halfSize.z));

            // Top
            DrawGizmosLine(new Vector3(+halfSize.x, +halfSize.y, +halfSize.z), new Vector3(+halfSize.x, +halfSize.y, -halfSize.z));
            DrawGizmosLine(new Vector3(+halfSize.x, +halfSize.y, -halfSize.z), new Vector3(-halfSize.x, +halfSize.y, -halfSize.z));
            DrawGizmosLine(new Vector3(-halfSize.x, +halfSize.y, -halfSize.z), new Vector3(-halfSize.x, +halfSize.y, +halfSize.z));
            DrawGizmosLine(new Vector3(-halfSize.x, +halfSize.y, +halfSize.z), new Vector3(+halfSize.x, +halfSize.y, +halfSize.z));

            // Side lines
            int steps = Mathf.Abs(Mathf.FloorToInt(angle / 36)) + 10;

            for (int i = 0; i < steps; i++)
            {
                float y0 = Mathf.Lerp(-halfSize.y, +halfSize.y, (float) (i + 0) / steps);
                float y1 = Mathf.Lerp(-halfSize.y, +halfSize.y, (float) (i + 1) / steps);

                DrawGizmosLine(new Vector3(+halfSize.x, y0, +halfSize.z), new Vector3(+halfSize.x, y1, +halfSize.z));
                DrawGizmosLine(new Vector3(+halfSize.x, y0, -halfSize.z), new Vector3(+halfSize.x, y1, -halfSize.z));
                DrawGizmosLine(new Vector3(-halfSize.x, y0, +halfSize.z), new Vector3(-halfSize.x, y1, +halfSize.z));
                DrawGizmosLine(new Vector3(-halfSize.x, y0, -halfSize.z), new Vector3(-halfSize.x, y1, -halfSize.z));
            }
        }

        private void DrawGizmosLine(Vector3 point0, Vector3 point1)
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
