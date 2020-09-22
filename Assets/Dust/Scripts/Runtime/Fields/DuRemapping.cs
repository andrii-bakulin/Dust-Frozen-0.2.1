using UnityEngine;

namespace DustEngine
{
    [System.Serializable]
    public class DuRemapping
    {
        public enum ContourMode
        {
            None = 0,
            Curve = 1,
            Step = 2,
        }

        public enum ColorMode
        {
            Color = 0,
            Gradient = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_RemapForceEnabled = true;
        public bool remapForceEnabled
        {
            get => m_RemapForceEnabled;
            set => m_RemapForceEnabled = value;
        }

        [SerializeField]
        private float m_Strength = 1.0f;
        public float strength
        {
            get => m_Strength;
            set => m_Strength = value;
        }

        [SerializeField]
        private float m_InnerOffset = 0.5f;
        public float innerOffset
        {
            get => m_InnerOffset;
            set => m_InnerOffset = ObjectNormalizer.InnerOffset(value);
        }

        [SerializeField]
        private bool m_Invert = false;
        public bool invert
        {
            get => m_Invert;
            set => m_Invert = value;
        }

        [SerializeField]
        private float m_Min = 0.0f;
        public float min
        {
            get => m_Min;
            set => m_Min = value;
        }

        [SerializeField]
        private float m_Max = 1.0f;
        public float max
        {
            get => m_Max;
            set => m_Max = value;
        }

        [SerializeField]
        private bool m_ClampMinEnabled = true;
        public bool clampMinEnabled
        {
            get => m_ClampMinEnabled;
            set => m_ClampMinEnabled = value;
        }

        [SerializeField]
        private bool m_ClampMaxEnabled = true;
        public bool clampMaxEnabled
        {
            get => m_ClampMaxEnabled;
            set => m_ClampMaxEnabled = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_ContourMultiplier = 1.0f;
        public float contourMultiplier
        {
            get => m_ContourMultiplier;
            set => m_ContourMultiplier = value;
        }

        [SerializeField]
        private ContourMode m_ContourMode = ContourMode.None;
        public ContourMode contourMode
        {
            get => m_ContourMode;
            set => m_ContourMode = value;
        }

        [SerializeField]
        private int m_ContourSteps = 1;
        public int contourSteps
        {
            get => m_ContourSteps;
            set => m_ContourSteps = ObjectNormalizer.ContourSteps(value);
        }

        [SerializeField]
        private AnimationCurve m_ContourSpline = DuAnimationCurve.StraightLine01();
        public AnimationCurve contourSpline
        {
            get => m_ContourSpline;
            set => m_ContourSpline = ObjectNormalizer.ContourSpline(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_RemapColorEnabled = false;
        public bool remapColorEnabled
        {
            get => m_RemapColorEnabled;
            set => m_RemapColorEnabled = value;
        }

        [SerializeField]
        private ColorMode m_ColorMode = ColorMode.Color;
        public ColorMode colorMode
        {
            get => m_ColorMode;
            set => m_ColorMode = value;
        }

        [SerializeField]
        protected Color m_Color = Color.white;
        public Color color
        {
            get => m_Color;
            set => m_Color = value;
        }

        [SerializeField]
        protected Gradient m_Gradient;
        public Gradient gradient
        {
            get => m_Gradient;
            set => m_Gradient = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public float MapValue(float inWeight)
        {
            if (!remapForceEnabled)
                return inWeight;

            //----------------------------------------------------------------------------------------------------------

            float inMin = 0f;
            float inMax = 1f - innerOffset;

            if (Mathf.Approximately(inMin, inMax))
                inMax = 0.0001f;

            float outMin;
            float outMax;

            if (!invert)
            {
                outMin = min;
                outMax = Mathf.LerpUnclamped(min, max, strength);
            }
            else
            {
                outMin = 1f - min;
                outMax = Mathf.LerpUnclamped(1f - min, 1f - max, strength);
            }

            if (innerOffset > 0f && inWeight > inMax)
                inWeight = inMax;

            float outWeight = DuMath.Map(inMin, inMax, outMin, outMax, inWeight);

            //----------------------------------------------------------------------------------------------------------
            // Clamp values if need

            if (clampMinEnabled)
                outWeight = Mathf.Max(outWeight, Mathf.Min(min, max)); // 2nd argument: find totally min value

            if (clampMaxEnabled)
                outWeight = Mathf.Min(outWeight, Mathf.Max(min, max)); // 2nd argument: find totally max value

            //----------------------------------------------------------------------------------------------------------
            // Contour

            switch (contourMode)
            {
                case ContourMode.None:
                    // Nothing need to do
                    break;

                case ContourMode.Step:
                    outWeight = DuMath.Step(outWeight, contourSteps, outMin, outMax);
                    break;

                case ContourMode.Curve:
                {
                    float weightNormalized = DuMath.Map(outMin, outMax, 0f, 1f, outWeight);

                    weightNormalized = contourSpline.Evaluate(weightNormalized);
                    outWeight = DuMath.Map01To(outMin, outMax, weightNormalized);
                    break;
                }
            }

            outWeight *= contourMultiplier;

            //----------------------------------------------------------------------------------------------------------

            return outWeight;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class ObjectNormalizer
        {
            public static float InnerOffset(float value)
            {
                return Mathf.Clamp01(value);
            }

            public static int ContourSteps(int value)
            {
                return Mathf.Max(1, value);
            }

            public static AnimationCurve ContourSpline(AnimationCurve curve)
            {
                curve.ClampTime(0f, 1f, true);
                curve.ClampValues(0f, 1f);
                return curve;
            }
        }
    }
}
