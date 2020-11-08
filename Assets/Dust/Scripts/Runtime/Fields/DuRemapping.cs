﻿using UnityEngine;

namespace DustEngine
{
    [System.Serializable]
    public class DuRemapping : DuDynamicStateInterface
    {
        public enum PostReshapeMode
        {
            None = 0,
            Step = 1,
            Curve = 2,
        }

        public enum ColorMode
        {
            Ignore = 0,
            Color = 1,
            Gradient = 2,
            Rainbow = 3,
            RandomColor = 4,
            RandomColorInRange = 5,
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
        private float m_PostPower = 1.0f;
        public float postPower
        {
            get => m_PostPower;
            set => m_PostPower = value;
        }

        [SerializeField]
        private PostReshapeMode m_PostReshapeMode = PostReshapeMode.None;
        public PostReshapeMode postReshapeMode
        {
            get => m_PostReshapeMode;
            set => m_PostReshapeMode = value;
        }

        [SerializeField]
        private int m_PostStepsCount = 1;
        public int postStepsCount
        {
            get => m_PostStepsCount;
            set => m_PostStepsCount = ObjectNormalizer.PostStepsCount(value);
        }

        [SerializeField]
        private AnimationCurve m_PostCurve = DuAnimationCurve.StraightLine01();
        public AnimationCurve postCurve
        {
            get => m_PostCurve;
            set => m_PostCurve = ObjectNormalizer.PostCurve(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private ColorMode m_ColorMode = ColorMode.Color;
        public ColorMode colorMode
        {
            get => m_ColorMode;
            set => m_ColorMode = value;
        }

        [SerializeField]
        protected Color m_Color = new Color(0.0f, 0.5f, 1.0f);
        public Color color
        {
            get => m_Color;
            set => m_Color = value;
        }

        [SerializeField]
        protected Gradient m_Gradient = DuGradient.CreateBlackToColor(new Color(0.0f, 0.5f, 1.0f));
        public Gradient gradient
        {
            get => m_Gradient;
            set => m_Gradient = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_RainbowMinOffset = 0.0f;
        public float rainbowMinOffset
        {
            get => m_RainbowMinOffset;
            set => m_RainbowMinOffset = value;
        }

        [SerializeField]
        private float m_RainbowMaxOffset = 1.0f;
        public float rainbowMaxOffset
        {
            get => m_RainbowMaxOffset;
            set => m_RainbowMaxOffset = value;
        }

        [SerializeField]
        private bool m_RainbowRepeat = false;
        public bool rainbowRepeat
        {
            get => m_RainbowRepeat;
            set => m_RainbowRepeat = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected Color m_RandomMinColor = Color.black;
        public Color randomMinColor
        {
            get => m_RandomMinColor;
            set => m_RandomMinColor = value;
        }

        [SerializeField]
        protected Color m_RandomMaxColor = Color.red;
        public Color randomMaxColor
        {
            get => m_RandomMaxColor;
            set => m_RandomMaxColor = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, remapForceEnabled);

            if (remapForceEnabled)
            {
                DuDynamicState.Append(ref dynamicState, ++seq, strength);
                DuDynamicState.Append(ref dynamicState, ++seq, innerOffset);
                DuDynamicState.Append(ref dynamicState, ++seq, invert);
                DuDynamicState.Append(ref dynamicState, ++seq, min);
                DuDynamicState.Append(ref dynamicState, ++seq, max);
                DuDynamicState.Append(ref dynamicState, ++seq, clampMinEnabled);
                DuDynamicState.Append(ref dynamicState, ++seq, clampMaxEnabled);

                DuDynamicState.Append(ref dynamicState, ++seq, postPower);
                DuDynamicState.Append(ref dynamicState, ++seq, postReshapeMode);
                DuDynamicState.Append(ref dynamicState, ++seq, postStepsCount);
                DuDynamicState.Append(ref dynamicState, ++seq, postCurve);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DuDynamicState.Append(ref dynamicState, ++seq, colorMode);

            switch (colorMode)
            {
                case ColorMode.Ignore:
                case ColorMode.RandomColor:
                    // none
                    break;

                case ColorMode.Color:
                    DuDynamicState.Append(ref dynamicState, ++seq, color);
                    break;

                case ColorMode.Gradient:
                    DuDynamicState.Append(ref dynamicState, ++seq, gradient);
                    break;

                case ColorMode.Rainbow:
                    DuDynamicState.Append(ref dynamicState, ++seq, rainbowMinOffset);
                    DuDynamicState.Append(ref dynamicState, ++seq, rainbowMaxOffset);
                    DuDynamicState.Append(ref dynamicState, ++seq, rainbowRepeat);
                    break;

                case ColorMode.RandomColorInRange:
                    DuDynamicState.Append(ref dynamicState, ++seq, randomMinColor);
                    DuDynamicState.Append(ref dynamicState, ++seq, randomMaxColor);
                    break;
            }

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public float MapValue(float inValue)
        {
            if (!remapForceEnabled)
                return inValue;

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

            if (innerOffset > 0f && inValue > inMax)
                inValue = inMax;

            float outValue = DuMath.Fit(inMin, inMax, outMin, outMax, inValue);

            //----------------------------------------------------------------------------------------------------------
            // Clamp values if need

            if (clampMinEnabled)
                outValue = Mathf.Max(outValue, Mathf.Min(min, max)); // 2nd argument: find totally min value

            if (clampMaxEnabled)
                outValue = Mathf.Min(outValue, Mathf.Max(min, max)); // 2nd argument: find totally max value

            //----------------------------------------------------------------------------------------------------------
            // Post Reshape

            switch (postReshapeMode)
            {
                case PostReshapeMode.None:
                    // Nothing need to do
                    break;

                case PostReshapeMode.Step:
                    outValue = DuMath.Step(outValue, postStepsCount, outMin, outMax);
                    break;

                case PostReshapeMode.Curve:
                {
                    float valueNormalized = DuMath.Fit(outMin, outMax, 0f, 1f, outValue);

                    valueNormalized = postCurve.Evaluate(valueNormalized);
                    outValue = DuMath.Fit01To(outMin, outMax, valueNormalized);
                    break;
                }
            }

            outValue *= postPower;

            //----------------------------------------------------------------------------------------------------------

            return outValue;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class ObjectNormalizer
        {
            public static float InnerOffset(float value)
            {
                return Mathf.Clamp01(value);
            }

            public static int PostStepsCount(int value)
            {
                return Mathf.Max(1, value);
            }

            public static AnimationCurve PostCurve(AnimationCurve curve)
            {
                curve.duClamp01TimeAndValues(true);
                return curve;
            }
        }
    }
}
