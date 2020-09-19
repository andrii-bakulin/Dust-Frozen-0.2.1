﻿using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Deformers/Wave Deformer")]
    [ExecuteInEditMode]
    public class DuWaveDeformer : DuDeformer
    {
        public enum GizmosQuality
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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_GizmosSize = 3f;
        public float gizmosSize
        {
            get => m_GizmosSize;
            set => m_GizmosSize = value;
        }

        [SerializeField]
        private GizmosQuality m_GizmosQuality = GizmosQuality.Medium;
        public GizmosQuality gizmosQuality
        {
            get => m_GizmosQuality;
            set => m_GizmosQuality = value;
        }

        [SerializeField]
        private bool m_GizmosAnimated = false;
        public bool gizmosAnimated
        {
            get => m_GizmosAnimated;
            set => m_GizmosAnimated = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private float m_TimeSinceStart;
        private float m_TimerForEditor;

        //------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Deformers/Wave")]
        public static void AddComponent()
        {
            AddDeformerComponentByType(typeof(DuWaveDeformer));
        }
#endif

        //------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnEnable()
        {
            if (!Application.isPlaying)
            {
                EditorUpdateReset();

                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
        }

        void OnDisable()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.update -= EditorUpdate;
            }
        }

        void EditorUpdate()
        {
            float deltaTime;

            if (!EditorUpdateTick(out deltaTime))
                return;

            if (gizmosAnimated)
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

        //------------------------------------------------------------------------------------------------------------------

        public override string DeformerName()
        {
            return "Wave";
        }

        public override bool DeformPoint(ref Vector3 localPosition, float strength = 1f)
        {
            float distance = DuMath.Length(localPosition.x, localPosition.z);

            if (DuMath.IsNotZero(linearFalloff) && distance >= linearFalloff)
                return false;

            float sinOffset = distance * frequency - offset - (m_TimeSinceStart + m_TimerForEditor) * animationSpeed;
            float offsetY = Mathf.Sin(DuConstants.PI2 * sinOffset) * amplitude / 2f;

            if (DuMath.IsNotZero(linearFalloff))
                offsetY *= Mathf.Clamp01((linearFalloff - distance) / linearFalloff);

            localPosition.y += offsetY * strength;
            return true;
        }

        private void Update()
        {
            if (!Application.isPlaying)
                return;

            m_TimeSinceStart += Time.deltaTime;
        }

        //------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawDeformerGizmos()
        {
            if (DuMath.IsZero(frequency))
                return;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = k_GizmosColorMain;

            int segments;

            switch (gizmosQuality)
            {
                default:
                case GizmosQuality.Low:       segments = 12; break;
                case GizmosQuality.Medium:    segments = 24; break;
                case GizmosQuality.High:      segments = 48; break;
                case GizmosQuality.ExtraHigh: segments = 96; break;
            }

            Vector3 zeroPoint = new Vector3(-gizmosSize / 2f, 0, -gizmosSize / 2f);

            float delta = gizmosSize / segments;

            for (int s0 = 0; s0 <= segments; s0++)
            for (int s1 = 0; s1 < segments; s1++)
            {
                Vector3 point0, point1;

                point0 = zeroPoint + new Vector3(s1 * delta, 0, s0 * delta);
                point1 = zeroPoint + new Vector3((s1 + 1) * delta, 0, s0 * delta);

                DeformPoint(ref point0);
                DeformPoint(ref point1);

                Gizmos.DrawLine(point0, point1);

                point0 = new Vector3(point0.z, point0.y, point0.x);
                point1 = new Vector3(point1.z, point1.y, point1.x);

                Gizmos.DrawLine(point0, point1);
            }
        }
#endif
    }
}