using System;
using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    [Serializable]
    public class DuFieldsMap
    {
        [Serializable]
        public class FieldRecord
        {
            public enum BlendMode
            {
                Normal = 0,
                Add = 1,
                Subtract = 2,
                Multiply = 3,
                Min = 4,
                Max = 5,
            }

            [SerializeField]
            private bool m_Enabled = true;
            public bool enabled
            {
                get => m_Enabled;
                set => m_Enabled = value;
            }

            [SerializeField]
            private DuField m_Field = null;
            public DuField field
            {
                get => m_Field;
                set => m_Field = value;
            }

            [SerializeField]
            private BlendMode m_BlendMode = BlendMode.Normal;
            public BlendMode blend
            {
                get => m_BlendMode;
                set => m_BlendMode = value;
            }

            [SerializeField]
            private bool m_CalculateValue = true;
            public bool calculateValue
            {
                get => m_CalculateValue;
                set => m_CalculateValue = value;
            }

            [SerializeField]
            private bool m_CalculateColor = true;
            public bool calculateColor
            {
                get => m_CalculateColor;
                set => m_CalculateColor = value;
            }

            [SerializeField]
            private float m_Intensity = 1f;
            public float intensity
            {
                get => m_Intensity;
                set => m_Intensity = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private bool m_CalculateValues = true;
        public bool calculateValues => m_CalculateValues;

        private bool m_CalculateColors = true;
        public bool calculateColors => m_CalculateColors;

        [SerializeField]
        protected List<FieldRecord> m_Fields = new List<FieldRecord>();
        public List<FieldRecord> fields => m_Fields;

        //--------------------------------------------------------------------------------------------------------------

        private DuFieldsMap()
        {
        }

        public static DuFieldsMap WeightsFieldsMap()
        {
            var duFieldsMap = new DuFieldsMap();
            duFieldsMap.m_CalculateValues = true;
            duFieldsMap.m_CalculateColors = false;
            return duFieldsMap;
        }

        public static DuFieldsMap WeightsAndColorsFieldsMap()
        {
            var duFieldsMap = new DuFieldsMap();
            duFieldsMap.m_CalculateValues = true;
            duFieldsMap.m_CalculateColors = true;
            return duFieldsMap;
        }

        //--------------------------------------------------------------------------------------------------------------

        private DuField.Point m_CalcFieldPoint = new DuField.Point();

        public bool HasFields()
        {
            return fields.Count > 0;
        }

        //--------------------------------------------------------------------------------------------------------------

        public bool Calculate(Vector3 worldPosition, float sequenceOffset, out float weight)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = sequenceOffset;

            bool result = Calculate(m_CalcFieldPoint);

            weight = m_CalcFieldPoint.outValue;

            return result;
        }

        public bool Calculate(Vector3 worldPosition, float sequenceOffset, out float weight, out Color color)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = sequenceOffset;

            bool result = Calculate(m_CalcFieldPoint);

            weight = m_CalcFieldPoint.outValue;
            color = m_CalcFieldPoint.outColor;

            return result;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool Calculate(DuField.Point fieldPoint)
        {
            // Start color is BLACK + alpha is ZERO
            // Because result will apply by alpha value
            fieldPoint.outColor = new Color(0f, 0f, 0f, 0f);
            fieldPoint.outValue = 0f;

            // Default return state
            if (fields.Count == 0)
            {
                fieldPoint.outValue = 1f;
                return false;
            }

            int fieldsApplied = 0;

            foreach (FieldRecord fieldRecord in fields)
            {
                if (Dust.IsNull(fieldRecord) || !fieldRecord.enabled || Dust.IsNull(fieldRecord.field))
                    continue;

                // Why here? Because it same magic in Cinema4D :)
                // For example if no one fields enabled in FieldsMap list
                //     -> then it think like list is empty and default (start) value is 1.0f
                // But if some of fields enabled in list, but object disabled in hierarchy
                //     -> then it count as 1+ field applied and default (start) value is 0.0f
                fieldsApplied++;

                // I don't check (!fieldRecord.field<Du..Field>.enabled) because I think as script always enabled!
                //          Of course you can manually set this flag to FALSE, but better don't do this!
                //          Also this object(s) will be forced ACTIVATED in UnityEditor mode.
                //          If you want temporary disable object then required SetActive(false) for it's gameObject.
                // PS: this logic same for Effectors and Fields

                if (!fieldRecord.field.gameObject.activeInHierarchy)
                    continue;

                float fieldWeight = fieldRecord.field.GetWeightForFieldPoint(fieldPoint);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                if (calculateValues && fieldRecord.calculateValue)
                {
                    float afterBlendWeight;

                    switch (fieldRecord.blend)
                    {
                        default:
                        case FieldRecord.BlendMode.Normal:
                            afterBlendWeight = fieldWeight;
                            break;

                        case FieldRecord.BlendMode.Add:
                            afterBlendWeight = fieldPoint.outValue + fieldWeight;
                            break;

                        case FieldRecord.BlendMode.Subtract:
                            afterBlendWeight = fieldPoint.outValue - fieldWeight;
                            break;

                        case FieldRecord.BlendMode.Multiply:
                            afterBlendWeight = fieldPoint.outValue * fieldWeight;
                            break;

                        case FieldRecord.BlendMode.Min:
                            afterBlendWeight = Mathf.Min(fieldPoint.outValue, fieldWeight);
                            break;

                        case FieldRecord.BlendMode.Max:
                            afterBlendWeight = Mathf.Max(fieldPoint.outValue, fieldWeight);
                            break;
                    }

                    fieldPoint.outValue = Mathf.LerpUnclamped(fieldPoint.outValue, afterBlendWeight, fieldRecord.intensity);
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Calculate new color value

                if (calculateColors && fieldRecord.calculateColor && fieldRecord.field.IsAllowGetFieldColor())
                {
                    Color fieldColor = fieldRecord.field.GetFieldColor(fieldPoint, fieldWeight);
                    Color blendedColor;

                    switch (fieldRecord.blend)
                    {
                        default:
                        case FieldRecord.BlendMode.Normal:
                            blendedColor = DuColorBlend.AlphaBlend(fieldPoint.outColor, fieldColor);
                            break;

                        case FieldRecord.BlendMode.Add:
                            blendedColor = DuColorBlend.Add(fieldPoint.outColor, fieldColor);
                            break;

                        case FieldRecord.BlendMode.Subtract:
                            blendedColor = DuColorBlend.Subtract(fieldPoint.outColor, fieldColor);
                            break;

                        case FieldRecord.BlendMode.Multiply:
                            blendedColor = DuColorBlend.Multiply(fieldPoint.outColor, fieldColor);
                            break;

                        case FieldRecord.BlendMode.Min:
                            blendedColor = DuColorBlend.Min(fieldPoint.outColor, DuColorBlend.AlphaBlend(fieldPoint.outColor, fieldColor));
                            break;

                        case FieldRecord.BlendMode.Max:
                            blendedColor = DuColorBlend.Max(fieldPoint.outColor, DuColorBlend.AlphaBlend(fieldPoint.outColor, fieldColor));
                            break;
                    }

                    fieldPoint.outColor = Color.Lerp(fieldPoint.outColor, blendedColor, fieldRecord.intensity);
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            }

            if (fieldsApplied == 0)
            {
                fieldPoint.outValue = 1f;
                return false;
            }

            return true;
        }

        public FieldRecord AddField(DuField field)
        {
            var fieldRecord = new FieldRecord
            {
                field = field,
                enabled = true,
                intensity = 1f,
                blend = FieldRecord.BlendMode.Normal,
            };

            fields.Add(fieldRecord);

            return fieldRecord;
        }
    }
}
