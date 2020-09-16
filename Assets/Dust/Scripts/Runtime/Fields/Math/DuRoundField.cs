using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Round Field")]
    public class DuRoundField : DuField
    {
        public enum RoundMode
        {
            Round = 0,
            Floor = 1,
            Ceil = 2,
        }

        [SerializeField]
        private RoundMode m_RoundMode = RoundMode.Round;
        public RoundMode roundMode
        {
            get => m_RoundMode;
            set => m_RoundMode = value;
        }

        [SerializeField]
        private float m_Distance = 0.2f;
        public float distance
        {
            get => m_Distance;
            set => m_Distance = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Math Fields/Round")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuRoundField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public float RoundValue(float value)
        {
            if (DuMath.IsZero(distance))
                return 0f;

            switch (roundMode)
            {
                case RoundMode.Round:
                    value = Mathf.Round(value / distance) * distance;
                    break;

                case RoundMode.Floor:
                    value = Mathf.Floor(value / distance) * distance;
                    break;

                case RoundMode.Ceil:
                    value = Mathf.Ceil(value / distance) * distance;
                    break;
            }

            return value;
        }

        public override string FieldName()
        {
            return "Round";
        }

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            return RoundValue(fieldPoint.outPower);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool IsAllowGetFieldColor()
        {
            return true;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            Color color = fieldPoint.outColor;
            color.r = RoundValue(color.r);
            color.g = RoundValue(color.g);
            color.b = RoundValue(color.b);
            color.a = RoundValue(color.a);
            return color;
        }
    }
}
