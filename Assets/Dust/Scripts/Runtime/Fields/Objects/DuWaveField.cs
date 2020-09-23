﻿using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Object Fields/Wave Field")]
    [ExecuteInEditMode]
    public class DuWaveField : DuObjectField
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
        private float m_Size = 1f;
        public float size
        {
            get => m_Size;
            set => m_Size = value;
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

        private float m_TimeSinceStart;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Object Fields/Wave")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuWaveField));
        }
#endif

        //------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        private float m_TimerForEditor;

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

            if (gizmoAnimated)
            {
                m_TimerForEditor += deltaTime;

                DustGUIRuntime.ForcedRedrawSceneView();
            }
            else
            {
                m_TimerForEditor = 0f;
            }
        }
#endif

        void Update()
        {
#if UNITY_EDITOR
            if (isEditorUpdatesEnabled) return;
#endif

            if (DuMath.IsNotZero(animationSpeed))
                m_TimeSinceStart += Time.deltaTime;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FieldName()
        {
            return "Wave";
        }

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            // Convert to Axis X+ (xp) space
            var xpLocalPosition = DuAxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            return GetPowerForLocalPositionInAxisXPlus(xpLocalPosition);
        }

        internal float GetPowerForLocalPositionInAxisXPlus(Vector3 xpLocalPosition)
        {
            if (DuMath.IsZero(size))
                return remapping.MapValue(0.5f);

            float distance = DuMath.Length(xpLocalPosition.y, xpLocalPosition.z);

            if (DuMath.IsNotZero(linearFalloff) && distance >= linearFalloff)
                return remapping.MapValue(0.5f);

            float timeOffset = m_TimeSinceStart;

#if UNITY_EDITOR
            if (gizmoAnimated)
            {
                timeOffset += m_TimerForEditor;
            }
#endif

            float sinOffset = distance / size - (offset + 0.75f) - timeOffset * animationSpeed;
            float waveOffset = Mathf.Sin(DuConstants.PI2 * sinOffset) * amplitude;

            if (DuMath.IsNotZero(linearFalloff))
                waveOffset *= Mathf.Clamp01((linearFalloff - distance) / linearFalloff);

            // Convert waveOffset [-1..+1] to [0..1]))
            waveOffset = DuMath.Map(-1f, +1f, 0f, 1f, waveOffset);

            return remapping.MapValue(waveOffset);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = GetGizmoColorRange1();

            int segments;

            switch (gizmoQuality)
            {
                default:
                case GizmoQuality.Low:       segments = 12; break;
                case GizmoQuality.Medium:    segments = 24; break;
                case GizmoQuality.High:      segments = 48; break;
                case GizmoQuality.ExtraHigh: segments = 96; break;
            }

            Vector3 zeroOffset = new Vector3(0, -gizmoSize / 2f, -gizmoSize / 2f);

            float delta = gizmoSize / segments;

            for (int s0 = 0; s0 <= segments; s0++)
            for (int s1 = 0; s1 < segments; s1++)
            {
                Vector3 pointY0, pointY1;
                Vector3 pointZ0, pointZ1;

                pointY0 = zeroOffset + new Vector3(0, (s1 + 0) * delta, s0 * delta);
                pointY1 = zeroOffset + new Vector3(0, (s1 + 1) * delta, s0 * delta);
                pointZ0 = new Vector3(pointY0.x, pointY0.z, pointY0.y);
                pointZ1 = new Vector3(pointY1.x, pointY1.z, pointY1.y);

                if (DuMath.IsNotZero(linearFalloff) && (pointY0.magnitude > linearFalloff || pointY1.magnitude > linearFalloff))
                    continue;

                pointY0.x = GetPowerForLocalPositionInAxisXPlus(pointY0);
                pointY1.x = GetPowerForLocalPositionInAxisXPlus(pointY1);
                pointZ0.x = GetPowerForLocalPositionInAxisXPlus(pointZ0);
                pointZ1.x = GetPowerForLocalPositionInAxisXPlus(pointZ1);

                pointY0 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pointY0);
                pointY1 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pointY1);
                pointZ0 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pointZ0);
                pointZ1 = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pointZ1);

                Gizmos.DrawLine(pointY0, pointY1);
                Gizmos.DrawLine(pointZ0, pointZ1);
            }

            if (DuMath.IsNotZero(linearFalloff))
            {
                Vector3 zeroPoint = Vector3.zero;
                zeroPoint.x = GetPowerForLocalPositionInAxisXPlus(new Vector3(0f, linearFalloff, linearFalloff)); // this point will be always outside falloff range
                zeroPoint = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, zeroPoint);

                DuGizmos.DrawCircle(linearFalloff, zeroPoint, direction, 32);
            }
        }
#endif

        private void Reset()
        {
            ResetDefaults();

            remapping.innerOffset = 0f;
        }
    }
}
