using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Remap Field")]
    public class DuRemapField : DuField
    {
        [SerializeField]
        private DuRemapping m_Remapping = new DuRemapping();
        public DuRemapping remapping => m_Remapping;

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, remapping.GetDynamicStateHashCode());

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Remap";
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
            return remapping.MapValue(fieldPoint.outPower);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Color

        public override bool IsAllowCalculateFieldColor()
        {
            return true;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            return GetFieldColorFromRemapping(remapping, powerByField);
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return true;
        }

        public override Gradient GetFieldColorPreview(out float intensity)
        {
            return GetFieldColorPreview(remapping, out intensity);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        private void Reset()
        {
            remapping.innerOffset = 0f;
        }
    }
}
