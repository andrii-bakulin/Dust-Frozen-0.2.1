using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Math Fields/Clamp Field")]
    public class DuClampField : DuField
    {
        public enum ClampMode
        {
            MinAndMax = 0,
            MinOnly = 1,
            MaxOnly = 2,
            NoClamp = 3,
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private ClampMode m_PowerClampMode = ClampMode.MinAndMax;
        public ClampMode powerClampMode
        {
            get => m_PowerClampMode;
            set => m_PowerClampMode = value;
        }

        [SerializeField]
        private float m_PowerClampMin = 0f;
        public float powerClampMin
        {
            get => m_PowerClampMin;
            set => m_PowerClampMin = value;
        }

        [SerializeField]
        private float m_PowerClampMax = 1f;
        public float powerClampMax
        {
            get => m_PowerClampMax;
            set => m_PowerClampMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private ClampMode m_ColorClampMode = ClampMode.MinAndMax;
        public ClampMode colorClampMode
        {
            get => m_ColorClampMode;
            set => m_ColorClampMode = value;
        }

        [SerializeField]
        private Color m_ColorClampMin = new Color(0f, 0f, 0f, 0f);
        public Color colorClampMin
        {
            get => m_ColorClampMin;
            set => m_ColorClampMin = value;
        }

        [SerializeField]
        private Color m_ColorClampMax = new Color(1f, 1f, 1f, 1f);
        public Color colorClampMax
        {
            get => m_ColorClampMax;
            set => m_ColorClampMax = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Math Fields/Clamp")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuClampField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, powerClampMode);
            DuDynamicState.Append(ref dynamicState, ++seq, powerClampMin);
            DuDynamicState.Append(ref dynamicState, ++seq, powerClampMax);

            DuDynamicState.Append(ref dynamicState, ++seq, colorClampMode);
            DuDynamicState.Append(ref dynamicState, ++seq, colorClampMin);
            DuDynamicState.Append(ref dynamicState, ++seq, colorClampMax);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Clamp";
        }

#if UNITY_EDITOR
        public override string FieldDynamicHint()
        {
            var parts = new List<string>();

            switch (powerClampMode)
            {
                case ClampMode.MinAndMax:
                    parts.Add("[" + powerClampMin.ToString("F2") + " .. " + powerClampMax.ToString("F2") + "]");
                    break;

                case ClampMode.MinOnly:
                    parts.Add("[" + powerClampMin.ToString("F2") + " .. ∞)");
                    break;

                case ClampMode.MaxOnly:
                    parts.Add("(∞ .. " + powerClampMax.ToString("F2") + "]");
                    break;

                case ClampMode.NoClamp:
                    parts.Add("(∞ .. ∞)");
                    break;
            }

            if (colorClampMode != ClampMode.NoClamp)
                parts.Add("Color");

            return string.Join(" + ", parts);
        }
#endif

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
            float value = fieldPoint.outPower;

            if (powerClampMode == ClampMode.MinAndMax || powerClampMode == ClampMode.MinOnly)
                value = Mathf.Max(value, powerClampMin);

            if (powerClampMode == ClampMode.MinAndMax || powerClampMode == ClampMode.MaxOnly)
                value = Mathf.Min(value, powerClampMax);

            return value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Color

        public override bool IsAllowCalculateFieldColor()
        {
            return true;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            Color color = fieldPoint.outColor;

            if (colorClampMode == ClampMode.MinAndMax || colorClampMode == ClampMode.MinOnly)
                color = DuColor.Max(color, colorClampMin);

            if (colorClampMode == ClampMode.MinAndMax || colorClampMode == ClampMode.MaxOnly)
                color = DuColor.Min(color, colorClampMax);

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
    }
}
