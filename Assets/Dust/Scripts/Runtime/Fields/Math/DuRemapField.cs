using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Remap Field")]
    public class DuRemapField : DuField
    {
        [SerializeField]
        private DuRemapping m_Remapping = new DuRemapping();
        public DuRemapping remapping => m_Remapping;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Math Fields/Remap")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuRemapField));
        }
#endif

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

#if UNITY_EDITOR
        public override string FieldDynamicHint()
        {
            return "";
        }
#endif

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
            return remapping.remapColorEnabled;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            return GetFieldColorFromRemapping(remapping, powerByField);
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return remapping.remapColorEnabled;
        }

        public override Gradient GetFieldColorPreview()
        {
            return GetFieldColorPreview(remapping);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        private void Reset()
        {
            remapping.color = DuColor.RandomColor();
            remapping.innerOffset = 0f;
        }
    }
}
