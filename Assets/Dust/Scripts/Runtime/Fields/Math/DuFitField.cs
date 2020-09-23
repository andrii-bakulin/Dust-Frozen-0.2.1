using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Fit Field")]
    public class DuFitField : DuField
    {
        [SerializeField]
        private float m_MinInput = 0f;
        public float minInput
        {
            get => m_MinInput;
            set => m_MinInput = value;
        }

        [SerializeField]
        private float m_MaxInput = 1f;
        public float maxInput
        {
            get => m_MaxInput;
            set => m_MaxInput = value;
        }

        [SerializeField]
        private float m_MinOutput = 0f;
        public float minOutput
        {
            get => m_MinOutput;
            set => m_MinOutput = value;
        }

        [SerializeField]
        private float m_MaxOutput = 1f;
        public float maxOutput
        {
            get => m_MaxOutput;
            set => m_MaxOutput = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Math Fields/Fit")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuFitField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, minInput);
            DuDynamicState.Append(ref dynamicState, ++seq, maxInput);
            DuDynamicState.Append(ref dynamicState, ++seq, minOutput);
            DuDynamicState.Append(ref dynamicState, ++seq, maxOutput);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FieldName()
        {
            return "Fit";
        }

#if UNITY_EDITOR
        public override string FieldEditorDynamicHint()
        {
            return "[" + minInput.ToString("F2") + " .. " + maxInput.ToString("F2") + "]"
                + " > "
                + "[" + minOutput.ToString("F2") + " .. " + maxOutput.ToString("F2") + "]";
        }
#endif

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            return DuMath.Map(minInput, maxInput, minOutput, maxOutput, fieldPoint.outPower);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool IsAllowGetFieldColor()
        {
            return false;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            return Color.magenta;
        }
    }
}
