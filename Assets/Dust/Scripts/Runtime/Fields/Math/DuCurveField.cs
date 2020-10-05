using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Curve Field")]
    public class DuCurveField : DuField
    {
        public enum CurveMode
        {
            Clamp = 0,
            Loop = 1,
            PingPong = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private AnimationCurve m_Shape = DuAnimationCurve.StraightLine01();
        public AnimationCurve shape
        {
            get => m_Shape;
            set => m_Shape = Normalizer.Shape(value);
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
        private CurveMode m_BeforeCurve = CurveMode.Clamp;
        public CurveMode beforeCurve
        {
            get => m_BeforeCurve;
            set => m_BeforeCurve = value;
        }

        [SerializeField]
        private CurveMode m_AfterCurve = CurveMode.Clamp;
        public CurveMode afterCurve
        {
            get => m_AfterCurve;
            set => m_AfterCurve = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private float m_OffsetDynamic;

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, shape);
            DuDynamicState.Append(ref dynamicState, ++seq, offset);
            DuDynamicState.Append(ref dynamicState, ++seq, animationSpeed);
            DuDynamicState.Append(ref dynamicState, ++seq, beforeCurve);
            DuDynamicState.Append(ref dynamicState, ++seq, afterCurve);
            DuDynamicState.Append(ref dynamicState, ++seq, m_OffsetDynamic);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Curve";
        }

        public override string FieldDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        public override DuFieldsMap.FieldRecord.BlendPowerMode GetBlendPowerMode()
        {
            return DuFieldsMap.FieldRecord.BlendPowerMode.Set;
        }

        public override DuFieldsMap.FieldRecord.BlendColorMode GetBlendColorMode()
        {
            return DuFieldsMap.FieldRecord.BlendColorMode.Set;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Power

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            return shape.Evaluate(RecalculateValue(fieldPoint.endPower));
        }

        //--------------------------------------------------------------------------------------------------------------
        // Color

        public override bool IsAllowCalculateFieldColor()
        {
            return true;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            Color color = fieldPoint.endColor;
            color.r = shape.Evaluate(RecalculateValue(color.r));
            color.g = shape.Evaluate(RecalculateValue(color.g));
            color.b = shape.Evaluate(RecalculateValue(color.b));
            color.Clamp01();
            return color;
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return false;
        }

        public override Gradient GetFieldColorPreview(out float intensity)
        {
            intensity = 0f;
            return null;
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        protected float RecalculateValue(float value)
        {
            value = value + m_OffsetDynamic + offset * animationSpeed;

            if (value < 0.0f)
            {
                switch (beforeCurve)
                {
                    default:
                    case CurveMode.Clamp:
                        value = 0f;
                        break;

                    case CurveMode.Loop:
                        value = Mathf.Repeat(value, 1f);
                        break;

                    case CurveMode.PingPong:
                        value = Mathf.PingPong(value, 1f);
                        break;
                }
            }
            else if (value > 1.0f)
            {
                switch (afterCurve)
                {
                    default:
                    case CurveMode.Clamp:
                        value = 1f;
                        break;

                    case CurveMode.Loop:
                        value = Mathf.Repeat(value, 1f);
                        break;

                    case CurveMode.PingPong:
                        value = Mathf.PingPong(value, 1f);
                        break;
                }
            }

            return value;
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_OffsetDynamic += Time.deltaTime * animationSpeed;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static AnimationCurve Shape(AnimationCurve curve)
            {
                curve.ClampTime(0f, 1f, true);
                curve.ClampValues(0f, 1f);
                return curve;
            }
        }
    }
}
