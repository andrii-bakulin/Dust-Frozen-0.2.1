using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Basic Fields/Noise Field")]
    public class DuNoiseField : DuSpaceField
    {
        public enum NoiseMode
        {
            Random = 0,
            Perlin = 1,
        }

        public enum NoiseSpace
        {
            Global = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private NoiseMode m_NoiseMode = NoiseMode.Random;
        public NoiseMode noiseMode
        {
            get => m_NoiseMode;
            set => m_NoiseMode = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_AnimationSpeed = 0f;
        public float animationSpeed
        {
            get => m_AnimationSpeed;
            set => m_AnimationSpeed = value;
        }

        [SerializeField]
        private float m_AnimationOffset = 0f;
        public float animationOffset
        {
            get => m_AnimationOffset;
            set => m_AnimationOffset = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private NoiseSpace m_NoiseSpace;
        public NoiseSpace noiseSpace
        {
            get => m_NoiseSpace;
            set => m_NoiseSpace = value;
        }

        [SerializeField]
        private float m_NoiseScale = 1f;
        public float noiseScale
        {
            get => m_NoiseScale;
            set => m_NoiseScale = Normalizer.NoiseScale(value);
        }

        [SerializeField]
        private float m_NoisePower = 1f;
        public float noisePower
        {
            get => m_NoisePower;
            set => m_NoisePower = Normalizer.NoisePower(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed = DuConstants.RANDOM_SEED_DEFAULT;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (m_Seed == value)
                    return;

                m_Seed = value;
                ResetStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuNoise m_DuNoise;

        private DuNoise duNoise => m_DuNoise ?? (m_DuNoise = new DuNoise(seed));

        private float m_OffsetDynamic;

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, m_NoiseMode);
            DuDynamicState.Append(ref dynamicState, ++seq, m_AnimationSpeed);
            DuDynamicState.Append(ref dynamicState, ++seq, m_AnimationOffset);
            DuDynamicState.Append(ref dynamicState, ++seq, m_NoiseSpace);
            DuDynamicState.Append(ref dynamicState, ++seq, m_NoiseScale);
            DuDynamicState.Append(ref dynamicState, ++seq, m_NoisePower);
            DuDynamicState.Append(ref dynamicState, ++seq, m_Seed);

            DuDynamicState.Append(ref dynamicState, ++seq, m_OffsetDynamic);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_OffsetDynamic += Time.deltaTime * animationSpeed;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Noise";
        }

        public override string FieldDynamicHint()
        {
            switch (noiseMode)
            {
                case NoiseMode.Random:
                    return "Random";

                case NoiseMode.Perlin:
                    return "Perlin";
            }

            return "";
        }

        public override void Calculate(DuField.Point fieldPoint, out DuField.Result result, bool calculateColor)
        {
            float noiseValue;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            switch (noiseMode)
            {
                case NoiseMode.Random:
                default:
                {
                    noiseValue = duNoise.Perlin1D(fieldPoint.inOffset * 1328.8767f, fieldPoint.inOffset * 2984.7353f, 2f);
                    break;
                }

                case NoiseMode.Perlin:
                {
                    Vector3 inSpaceOffset = fieldPoint.inPosition; // point in world space

                    if (noiseSpace == NoiseSpace.Local)
                        inSpaceOffset = transform.worldToLocalMatrix.MultiplyPoint(inSpaceOffset);

                    float animTotalOffset = m_OffsetDynamic + animationOffset;

                    if (DuMath.IsNotZero(noiseScale))
                        inSpaceOffset /= noiseScale;

                    noiseValue = duNoise.Perlin3D(inSpaceOffset, animTotalOffset, noisePower);
                    break;
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            result.fieldPower = remapping.MapValue(noiseValue);
            result.fieldColor = GetFieldColorFromRemapping(remapping, result.fieldPower, calculateColor);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            // Nothing for now...
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            ResetStates();
        }

        public void ResetStates()
        {
            m_DuNoise = null;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static float NoiseScale(float value)
            {
                return Mathf.Clamp(value, 0.0001f, float.MaxValue);
            }

            public static float NoisePower(float value)
            {
                return Mathf.Clamp(value, 0.0001f, float.MaxValue);
            }
        }
    }
}
