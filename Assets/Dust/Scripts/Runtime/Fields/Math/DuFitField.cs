using UnityEngine;

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
        // Basic

        public override string FieldName()
        {
            return "Fit";
        }

        public override string FieldDynamicHint()
        {
            return "[" + minInput.ToString("F2") + " .. " + maxInput.ToString("F2") + "]"
                + " > "
                + "[" + minOutput.ToString("F2") + " .. " + maxOutput.ToString("F2") + "]";
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

        public override void Calculate(DuField.Point fieldPoint, out DuField.Result result, bool calculateColor)
        {
            result.fieldPower = DuMath.Fit(minInput, maxInput, minOutput, maxOutput, fieldPoint.endPower);

            result.fieldColor = Color.clear;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Color

        public override bool IsAllowCalculateFieldColor()
        {
            return false;
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
    }
}
