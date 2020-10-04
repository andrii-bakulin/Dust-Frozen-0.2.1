﻿using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Basic Fields/Constant Field")]
    public class DuConstantField : DuField
    {
        [SerializeField]
        private float m_Power = 1f;
        public float power
        {
            get => m_Power;
            set => m_Power = value;
        }

        [SerializeField]
        private Color m_Color = Color.white;
        public Color color
        {
            get => m_Color;
            set => m_Color = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, power);
            DuDynamicState.Append(ref dynamicState, ++seq, color);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Basic

        public override string FieldName()
        {
            return "Constant";
        }

        public override string FieldDynamicHint()
        {
            return "Power " + power.ToString("F2");
        }

        //--------------------------------------------------------------------------------------------------------------
        // Power

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            return power;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Color

        public override bool IsAllowCalculateFieldColor()
        {
            return true;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            // Notice: ignore incoming powerByField value
            return color;
        }

#if UNITY_EDITOR
        public override bool IsHasFieldColorPreview()
        {
            return true;
        }

        public override Gradient GetFieldColorPreview(out float intensity)
        {
            intensity = color.a;
            return color.ToGradient();
        }
#endif
    }
}
