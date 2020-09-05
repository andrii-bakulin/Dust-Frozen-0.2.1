﻿using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Animation/Vibrate")]
    public class DuVibrate : DuMonoBehaviour
    {
        internal const float k_MinScaleValue = 0.0001f;

        public enum TransformMode
        {
            Relative = 0,
            Absolute = 1,
            AppendToAnimation = 2,
        };

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_Uniform = false;
        public bool uniform
        {
            get => m_Uniform;
            set => m_Uniform = value;
        }

        [SerializeField]
        private int m_Seed = 0;
        public int seed
        {
            get => m_Seed;
            set => m_Seed = value;
        }

        [SerializeField]
        private float m_Force = 1f;
        public float force
        {
            get => m_Force;
            set => m_Force = Normalizer.Force(value);
        }

        [SerializeField]
        private bool m_Freeze = false;
        public bool freeze
        {
            get => m_Freeze;
            set => m_Freeze = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_PositionEnabled = false;
        public bool positionEnabled
        {
            get => m_PositionEnabled;
            set => m_PositionEnabled = value;
        }

        [SerializeField]
        private Vector3 m_PositionAmplitude = Vector3.one;
        public Vector3 positionAmplitude
        {
            get => m_PositionAmplitude;
            set => m_PositionAmplitude = value;
        }

        [SerializeField]
        private float m_PositionFrequency = 1f;
        public float positionFrequency
        {
            get => m_PositionFrequency;
            set => m_PositionFrequency = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_RotationEnabled = false;
        public bool rotationEnabled
        {
            get => m_RotationEnabled;
            set => m_RotationEnabled = value;
        }

        [SerializeField]
        private Vector3 m_RotationAmplitude = Vector3.up * 90f;
        public Vector3 rotationAmplitude
        {
            get => m_RotationAmplitude;
            set => m_RotationAmplitude = value;
        }

        [SerializeField]
        private float m_RotationFrequency = 1f;
        public float rotationFrequency
        {
            get => m_RotationFrequency;
            set => m_RotationFrequency = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ScaleEnabled = false;
        public bool scaleEnabled
        {
            get => m_ScaleEnabled;
            set => m_ScaleEnabled = value;
        }

        [SerializeField]
        private bool m_ScaleUniform = false;
        public bool scaleUniform
        {
            get => m_ScaleUniform;
            set => m_ScaleUniform = value;
        }

        [SerializeField]
        private Vector3 m_ScaleAmplitude = DuVector3.New(2f);
        public Vector3 scaleAmplitude
        {
            get => m_ScaleAmplitude;
            set => m_ScaleAmplitude = Normalizer.ScaleAmplitude(value);
        }

        [SerializeField]
        private float m_ScaleFrequency = 1f;
        public float scaleFrequency
        {
            get => m_ScaleFrequency;
            set => m_ScaleFrequency = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private TransformMode m_TransformMode = TransformMode.Relative;
        public TransformMode transformMode
        {
            get => m_TransformMode;
            set => m_TransformMode = value;
        }

        [SerializeField]
        private UpdateMode m_UpdateMode = UpdateMode.LateUpdate;
        public UpdateMode updateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private Vector3 m_LastDeltaPosition = Vector3.zero;
        private Vector3 m_LastDeltaRotation = Vector3.zero;
        private Vector3 m_LastMultScale = Vector3.one;

        public Vector3 lastDeltaPosition => m_LastDeltaPosition;
        public Vector3 lastDeltaRotation => m_LastDeltaRotation;
        public Vector3 lastMultScale => m_LastMultScale;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuNoise n_DuNoise;
        private DuNoise duNoise => n_DuNoise ?? (n_DuNoise = new DuNoise(seed));

        private float m_TimeSinceStart;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Animation/Vibrate")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Vibrate", typeof(DuVibrate));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            if (updateMode != UpdateMode.Update)
                return;

            UpdateState(Time.deltaTime);
        }

        void LateUpdate()
        {
            if (updateMode != UpdateMode.LateUpdate)
                return;

            UpdateState(Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (updateMode != UpdateMode.FixedUpdate)
                return;

            UpdateState(Time.fixedDeltaTime);
        }

        void UpdateState(float deltaTime)
        {
            if (!freeze)
                m_TimeSinceStart += deltaTime;

            force = Mathf.Clamp01(force);

            if (positionEnabled)
            {
                Vector3 deltaPosition = Vector3.zero;

                if (force > 0f)
                {
                    deltaPosition = positionAmplitude * force;

                    if (uniform)
                        deltaPosition *= Mathf.Sin(DuConstants.PI2 * m_TimeSinceStart * positionFrequency);
                    else
                        deltaPosition.Scale(duNoise.PerlinNoiseWideVector3(m_TimeSinceStart * positionFrequency));
                }

                switch (transformMode)
                {
                    case TransformMode.Relative:
                        transform.localPosition += deltaPosition - m_LastDeltaPosition;
                        break;

                    case TransformMode.Absolute:
                        transform.localPosition = deltaPosition;
                        break;

                    case TransformMode.AppendToAnimation:
                        transform.localPosition += deltaPosition;
                        break;
                }

                m_LastDeltaPosition = deltaPosition;
            }

            if (rotationEnabled)
            {
                Vector3 deltaRotation = Vector3.zero;

                if (force > 0f)
                {
                    deltaRotation = rotationAmplitude * force;

                    if (uniform)
                        deltaRotation *= Mathf.Sin(DuConstants.PI2 * m_TimeSinceStart * rotationFrequency);
                    else
                        deltaRotation.Scale(duNoise.PerlinNoiseWideVector3(m_TimeSinceStart * rotationFrequency));
                }

                switch (transformMode)
                {
                    case TransformMode.Relative:
                        transform.localEulerAngles += deltaRotation - m_LastDeltaRotation;
                        break;

                    case TransformMode.Absolute:
                        transform.localEulerAngles = deltaRotation;
                        break;

                    case TransformMode.AppendToAnimation:
                        transform.localEulerAngles += deltaRotation;
                        break;
                }

                m_LastDeltaRotation = deltaRotation;
            }

            if (scaleEnabled)
            {
                Vector3 multScale = Vector3.one;

                if (force > 0f)
                {
                    Vector3 noiseValue;

                    if (uniform)
                    {
                        noiseValue.x = noiseValue.y = noiseValue.z = Mathf.Sin(DuConstants.PI2 * m_TimeSinceStart * scaleFrequency);
                    }
                    else if (scaleUniform)
                    {
                        noiseValue.x = noiseValue.y = noiseValue.z = duNoise.PerlinNoiseWide(m_TimeSinceStart * scaleFrequency);
                    }
                    else
                    {
                        noiseValue = duNoise.PerlinNoiseWideVector3(m_TimeSinceStart * scaleFrequency);
                    }

                    multScale.x = CalcScaleValue(scaleAmplitude.x, noiseValue.x);
                    multScale.y = CalcScaleValue(scaleAmplitude.y, noiseValue.y);
                    multScale.z = CalcScaleValue(scaleAmplitude.z, noiseValue.z);

                    if (force < 1f)
                        multScale = Vector3.Lerp(Vector3.one, multScale, force);
                }

                switch (transformMode)
                {
                    case TransformMode.Relative:
                    {
                        Vector3 scale = transform.localScale;
                        scale.InverseScale(m_LastMultScale);
                        scale.Scale(multScale);
                        transform.localScale = scale;
                        break;
                    }

                    case TransformMode.Absolute:
                        transform.localScale = multScale;
                        break;

                    case TransformMode.AppendToAnimation:
                    {
                        Vector3 scale = transform.localScale;
                        scale.Scale(multScale);
                        transform.localScale = scale;
                        break;
                    }
                }

                m_LastMultScale = multScale;
            }
        }

        float CalcScaleValue(float amplitude, float noise)
        {
            if (amplitude < k_MinScaleValue)
                amplitude = k_MinScaleValue;

            if (noise >= 0f)
                return Mathf.Lerp(1f, amplitude, noise);

            return Mathf.Lerp(1f, 1f / amplitude, -noise);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        internal static class Normalizer
        {
            public static float Force(float value)
            {
                return Mathf.Clamp01(value);
            }

            public static Vector3 ScaleAmplitude(Vector3 value)
            {
                value = Vector3.Max(value, DuVector3.New(k_MinScaleValue));
                return value;
            }
        }
    }
}
