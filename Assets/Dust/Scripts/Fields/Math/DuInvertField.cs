using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Invert Field")]
    public class DuInvertField : DuField
    {
#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Math Fields/Invert")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuInvertField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string FieldName()
        {
            return "Invert";
        }

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            return 1f - fieldPoint.outPower;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool IsAllowGetFieldColor()
        {
            return true;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            return DuColor.InvertRGB(fieldPoint.outColor);
        }
    }
}
