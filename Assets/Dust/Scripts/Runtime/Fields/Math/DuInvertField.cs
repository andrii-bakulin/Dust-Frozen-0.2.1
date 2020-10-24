using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Invert Field")]
    public class DuInvertField : DuField
    {
        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, true);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Invert";
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

        public override void Calculate(DuField.Point fieldPoint, out DuField.Result result, bool calculateColor)
        {
            result.fieldPower = 1f - fieldPoint.endPower;

            result.fieldColor = calculateColor ? DuColor.InvertRGB(fieldPoint.endColor) : Color.clear;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override bool IsAllowCalculateFieldColor()
        {
            return true;
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return false;
        }

        public override Gradient GetFieldColorPreview(out float colorPower)
        {
            colorPower = 0f;
            return null;
        }
#endif
    }
}
