using UnityEngine;
using UnityEditor;

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
            set => m_Shape = Normalizer.ContourSpline(value);
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

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Math Fields/Curve")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuCurveField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string FieldName()
        {
            return "Curve";
        }

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            return shape.Evaluate(RecalculateValue(fieldPoint.outPower));
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool IsAllowGetFieldColor()
        {
            return true;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            Color color = fieldPoint.outColor;
            color.r = shape.Evaluate(RecalculateValue(color.r));
            color.g = shape.Evaluate(RecalculateValue(color.g));
            color.b = shape.Evaluate(RecalculateValue(color.b));
            color.Clamp01();
            return color;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected float RecalculateValue(float value)
        {
            value = value + (timeSinceStart + offset) * animationSpeed;

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
        // Normalizer

        public static class Normalizer
        {
            public static AnimationCurve ContourSpline(AnimationCurve curve)
            {
                curve.ClampTime(0f, 1f, true);
                curve.ClampValues(0f, 1f);
                return curve;
            }
        }
    }
}
