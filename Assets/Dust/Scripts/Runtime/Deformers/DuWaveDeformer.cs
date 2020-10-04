using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Deformers/Wave Deformer")]
    [ExecuteInEditMode]
    public class DuWaveDeformer : DuDeformer
    {
        public enum GizmoQuality
        {
            Low = 0,
            Medium = 1,
            High = 2,
            ExtraHigh = 3
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private float m_Amplitude = 0.5f;
        public float amplitude
        {
            get => m_Amplitude;
            set => m_Amplitude = value;
        }

        [SerializeField]
        private float m_Frequency = 1f;
        public float frequency
        {
            get => m_Frequency;
            set => m_Frequency = value;
        }

        [SerializeField]
        private float m_LinearFalloff = 0f;
        public float linearFalloff
        {
            get => m_LinearFalloff;
            set => m_LinearFalloff = value;
        }

        [SerializeField]
        private float m_Offset = 0f;
        public float offset
        {
            get => m_Offset;
            set => m_Offset = value;
        }

        [SerializeField]
        private float m_AnimationSpeed = 0f;
        public float animationSpeed
        {
            get => m_AnimationSpeed;
            set => m_AnimationSpeed = value;
        }

        [SerializeField]
        private Axis3xDirection m_Direction = Axis3xDirection.Y;
        public Axis3xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_GizmoSize = 3f;
        public float gizmoSize
        {
            get => m_GizmoSize;
            set => m_GizmoSize = value;
        }

        [SerializeField]
        private GizmoQuality m_GizmoQuality = GizmoQuality.Medium;
        public GizmoQuality gizmoQuality
        {
            get => m_GizmoQuality;
            set => m_GizmoQuality = value;
        }

        [SerializeField]
        private bool m_GizmoAnimated = false;
        public bool gizmoAnimated
        {
            get => m_GizmoAnimated;
            set => m_GizmoAnimated = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private float m_OffsetDynamic;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnEnable()
        {
            if (isEditorUpdatesEnabled)
            {
                EditorUpdateReset();

                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
        }

        void OnDisable()
        {
            if (isEditorUpdatesEnabled)
            {
                EditorApplication.update -= EditorUpdate;
            }
        }

        void EditorUpdate()
        {
            float deltaTime;

            if (!EditorUpdateTick(out deltaTime))
                return;

            if (gizmoAnimated && DuMath.IsNotZero(animationSpeed))
            {
                m_OffsetDynamic += deltaTime * animationSpeed;

                DustGUIRuntime.ForcedRedrawSceneView();
            }
        }
#endif

        void Update()
        {
#if UNITY_EDITOR
            if (isEditorUpdatesEnabled) return;
#endif

            m_OffsetDynamic += Time.deltaTime * animationSpeed;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string DeformerName()
        {
            return "Wave";
        }

#if UNITY_EDITOR
        public override string DeformerDynamicHint()
        {
            return "";
        }
#endif

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool DeformPoint(ref Vector3 localPosition, float strength = 1f)
        {
            // xp = x+
            var xpAxisPosition = DuAxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Deform logic (in X+ direction)

            float distance = DuMath.Length(xpAxisPosition.y, xpAxisPosition.z);

            if (DuMath.IsNotZero(linearFalloff) && distance >= linearFalloff)
                return false;

            float sinOffset = distance * frequency - (offset + 0.75f) - m_OffsetDynamic;
            float waveOffset = Mathf.Sin(DuConstants.PI2 * sinOffset) * amplitude / 2f;

            if (DuMath.IsNotZero(linearFalloff))
                waveOffset *= Mathf.Clamp01((linearFalloff - distance) / linearFalloff);

            xpAxisPosition.x += waveOffset * strength;

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

            DuDynamicState.Append(ref dynamicState, ++seq, amplitude);
            DuDynamicState.Append(ref dynamicState, ++seq, frequency);
            DuDynamicState.Append(ref dynamicState, ++seq, linearFalloff);
            DuDynamicState.Append(ref dynamicState, ++seq, offset);
            DuDynamicState.Append(ref dynamicState, ++seq, animationSpeed);
            DuDynamicState.Append(ref dynamicState, ++seq, direction);

            DuDynamicState.Append(ref dynamicState, ++seq, m_OffsetDynamic);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawDeformerGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = enabled ? k_GizmosColorActive : k_GizmosColorDisabled;

            int segments;

            switch (gizmoQuality)
            {
                default:
                case GizmoQuality.Low:       segments = 12; break;
                case GizmoQuality.Medium:    segments = 24; break;
                case GizmoQuality.High:      segments = 48; break;
                case GizmoQuality.ExtraHigh: segments = 96; break;
            }

            Vector3 zeroPoint = new Vector3(0, -gizmoSize / 2f, -gizmoSize / 2f);

            float delta = gizmoSize / segments;

            for (int s0 = 0; s0 <= segments; s0++)
            for (int s1 = 0; s1 < segments; s1++)
            {
                Vector3 pointY0, pointY1;
                Vector3 pointZ0, pointZ1;

                pointY0 = zeroPoint + new Vector3(0, (s1 + 0) * delta, s0 * delta);
                pointY1 = zeroPoint + new Vector3(0, (s1 + 1) * delta, s0 * delta);
                pointZ0 = new Vector3(pointY0.x, pointY0.z, pointY0.y); // invert Y-Z
                pointZ1 = new Vector3(pointY1.x, pointY1.z, pointY1.y); // invert Y-Z

                pointY0 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pointY0);
                pointY1 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pointY1);
                pointZ0 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pointZ0);
                pointZ1 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pointZ1);

                DeformPoint(ref pointY0);
                DeformPoint(ref pointY1);
                DeformPoint(ref pointZ0);
                DeformPoint(ref pointZ1);

                Gizmos.DrawLine(pointY0, pointY1);
                Gizmos.DrawLine(pointZ0, pointZ1);
            }

            DuGizmos.DrawCircle(linearFalloff, Vector3.zero, direction, 32);
        }
#endif
    }
}
