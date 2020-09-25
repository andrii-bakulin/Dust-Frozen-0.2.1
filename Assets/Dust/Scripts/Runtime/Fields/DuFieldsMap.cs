using System;
using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    [Serializable]
    public class DuFieldsMap : DuDynamicStateInterface
    {
        [Serializable]
        public class FieldRecord
        {
            public enum BlendPowerMode
            {
                Ignore = 0,
                Set = 1,
                Add = 2,
                Subtract = 3,
                Multiply = 4,
                Divide = 5,
                Min = 6,
                Max = 7,
            }

            public enum BlendColorMode
            {
                Ignore = 0,
                Set = 1,
                Blend = 2,
                Add = 3,
                Subtract = 4,
                Multiply = 5,
                Min = 6,
                Max = 7,
            }

            //--------------------------------------------------------------------------------------------------------------

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
            private BlendPowerMode m_BlendPowerMode = BlendPowerMode.Set;
            public BlendPowerMode blendPowerMode
            {
                get => m_BlendPowerMode;
                set => m_BlendPowerMode = value;
            }

            [SerializeField]
            private BlendColorMode m_BlendColorMode = BlendColorMode.Set;
            public BlendColorMode blendColorMode
            {
                get => m_BlendColorMode;
                set => m_BlendColorMode = value;
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

        [SerializeField]
        private bool m_CalculatePower;
        public bool calculatePower
        {
            get => m_CalculatePower;
            set => m_CalculatePower = value;
        }

        [SerializeField]
        private float m_DefaultPower = 0f;
        public float defaultPower
        {
            get => m_DefaultPower;
            set => m_DefaultPower = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_CalculateColor;
        public bool calculateColor
        {
            get => m_CalculateColor;
            set => m_CalculateColor = value;
        }

        [SerializeField]
        private Color m_DefaultColor = Color.black;
        public Color defaultColor
        {
            get => m_DefaultColor;
            set => m_DefaultColor = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected List<FieldRecord> m_Fields = new List<FieldRecord>();
        public List<FieldRecord> fields => m_Fields;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuField.Point m_CalcFieldPoint = new DuField.Point();

        //--------------------------------------------------------------------------------------------------------------

        public static DuFieldsMap Deformer()
        {
            return new DuFieldsMap(true, false);
        }

        public static DuFieldsMap Factory()
        {
            return new DuFieldsMap(true, true);
        }

        public static DuFieldsMap FieldsSpace()
        {
            return new DuFieldsMap(true, true);
        }

        public DuFieldsMap(bool calcPower, bool calcColor)
        {
            calculatePower = calcPower;
            calculateColor = calcColor;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, calculatePower);
            DuDynamicState.Append(ref dynamicState, ++seq, defaultPower);
            DuDynamicState.Append(ref dynamicState, ++seq, calculateColor);
            DuDynamicState.Append(ref dynamicState, ++seq, defaultColor);

            DuDynamicState.Append(ref dynamicState, ++seq, fields.Count);

            foreach (FieldRecord fieldRecord in fields)
            {
                // @WARNING!!! require sync code in: GetDynamicStateHashCode() + Calculate()

                if (Dust.IsNull(fieldRecord) || !fieldRecord.enabled || Dust.IsNull(fieldRecord.field))
                    continue;

                if (!fieldRecord.field.enabled || !fieldRecord.field.gameObject.activeInHierarchy)
                    continue;

                // @END

                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.enabled);
                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.field);
                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.blendPowerMode);
                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.blendColorMode);
                DuDynamicState.Append(ref dynamicState, ++seq, fieldRecord.intensity);
            }

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public bool HasFields()
        {
            return fields.Count > 0;
        }

        public FieldRecord.BlendPowerMode GetDefaultBlendPower()
        {
            return fields.Count == 0 ? FieldRecord.BlendPowerMode.Set : FieldRecord.BlendPowerMode.Max;
        }

        public FieldRecord.BlendColorMode GetDefaultBlendColor()
        {
            return FieldRecord.BlendColorMode.Blend;
        }

        //--------------------------------------------------------------------------------------------------------------

        public bool Calculate(Vector3 worldPosition, float sequenceOffset, out float power)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = sequenceOffset;

            bool result = Calculate(m_CalcFieldPoint);

            power = m_CalcFieldPoint.outPower;

            return result;
        }

        public bool Calculate(Vector3 worldPosition, float sequenceOffset, out float power, out Color color)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = sequenceOffset;

            bool result = Calculate(m_CalcFieldPoint);

            power = m_CalcFieldPoint.outPower;
            color = m_CalcFieldPoint.outColor;

            return result;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool Calculate(DuFactory factory, DuFactoryInstance factoryInstance, out float power)
        {
            if (fields.Count == 0)
            {
                power = 1f; // This is tricks just for factory-machine logics
                return false;
            }

            m_CalcFieldPoint.inPosition = factory.GetInstancePositionInWorldSpace(factoryInstance);
            m_CalcFieldPoint.inOffset = factoryInstance.offset;

            bool result = Calculate(m_CalcFieldPoint);

            power = m_CalcFieldPoint.outPower;

            return result;
        }

        public bool Calculate(DuFactory factory, DuFactoryInstance factoryInstance, out float power, out Color color)
        {
            if (fields.Count == 0)
            {
                power = 1f; // This is tricks just for factory-machine logics
                color = Color.black;
                return false;
            }

            m_CalcFieldPoint.inPosition = factory.GetInstancePositionInWorldSpace(factoryInstance);
            m_CalcFieldPoint.inOffset = factoryInstance.offset;

            bool result = Calculate(m_CalcFieldPoint);

            power = m_CalcFieldPoint.outPower;
            color = m_CalcFieldPoint.outColor;

            return result;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool Calculate(DuField.Point fieldPoint)
        {
            fieldPoint.outPower = calculatePower ? defaultPower : 0f;
            fieldPoint.outColor = calculateColor ? defaultColor : Color.black;

            if (fields.Count == 0)
                return false;

            foreach (FieldRecord fieldRecord in fields)
            {
                // @WARNING!!! require sync code in: GetDynamicStateHashCode() + Calculate()

                if (Dust.IsNull(fieldRecord) || !fieldRecord.enabled || Dust.IsNull(fieldRecord.field))
                    continue;

                if (!fieldRecord.field.enabled || !fieldRecord.field.gameObject.activeInHierarchy)
                    continue;

                // @END

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                float fieldPower = fieldRecord.field.GetPowerForFieldPoint(fieldPoint);

                if (calculatePower && fieldRecord.blendPowerMode != FieldRecord.BlendPowerMode.Ignore)
                {
                    float afterBlendPower;

                    switch (fieldRecord.blendPowerMode)
                    {
                        default:
                        case FieldRecord.BlendPowerMode.Set:
                            afterBlendPower = fieldPower;
                            break;

                        case FieldRecord.BlendPowerMode.Add:
                            afterBlendPower = fieldPoint.outPower + fieldPower;
                            break;

                        case FieldRecord.BlendPowerMode.Subtract:
                            afterBlendPower = fieldPoint.outPower - fieldPower;
                            break;

                        case FieldRecord.BlendPowerMode.Multiply:
                            afterBlendPower = fieldPoint.outPower * fieldPower;
                            break;

                        case FieldRecord.BlendPowerMode.Divide:
                            afterBlendPower = DuMath.IsNotZero(fieldPower) ? fieldPoint.outPower / fieldPower : 0f;
                            break;

                        case FieldRecord.BlendPowerMode.Min:
                            afterBlendPower = Mathf.Min(fieldPoint.outPower, fieldPower);
                            break;

                        case FieldRecord.BlendPowerMode.Max:
                            afterBlendPower = Mathf.Max(fieldPoint.outPower, fieldPower);
                            break;
                    }

                    fieldPoint.outPower = Mathf.LerpUnclamped(fieldPoint.outPower, afterBlendPower, fieldRecord.intensity);
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Calculate new color

                if (calculateColor && fieldRecord.blendColorMode != FieldRecord.BlendColorMode.Ignore && fieldRecord.field.IsAllowCalculateFieldColor())
                {
                    Color fieldColor = fieldRecord.field.GetFieldColor(fieldPoint, fieldPower);
                    Color blendedColor;

                    switch (fieldRecord.blendColorMode)
                    {
                        default:
                        case FieldRecord.BlendColorMode.Set:
                            blendedColor = fieldColor;
                            break;

                        case FieldRecord.BlendColorMode.Blend:
                            blendedColor = DuColorBlend.AlphaBlend(fieldPoint.outColor, fieldColor);
                            break;

                        case FieldRecord.BlendColorMode.Add:
                            blendedColor = DuColorBlend.Add(fieldPoint.outColor, fieldColor);
                            break;

                        case FieldRecord.BlendColorMode.Subtract:
                            blendedColor = DuColorBlend.Subtract(fieldPoint.outColor, fieldColor);
                            break;

                        case FieldRecord.BlendColorMode.Multiply:
                            blendedColor = DuColorBlend.Multiply(fieldPoint.outColor, fieldColor);
                            break;

                        case FieldRecord.BlendColorMode.Min:
                            blendedColor = DuColorBlend.Min(fieldPoint.outColor, DuColorBlend.AlphaBlend(fieldPoint.outColor, fieldColor));
                            break;

                        case FieldRecord.BlendColorMode.Max:
                            blendedColor = DuColorBlend.Max(fieldPoint.outColor, DuColorBlend.AlphaBlend(fieldPoint.outColor, fieldColor));
                            break;
                    }

                    fieldPoint.outColor = Color.Lerp(fieldPoint.outColor, blendedColor, fieldRecord.intensity);
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
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
                blendPowerMode = GetDefaultBlendPower(),
                blendColorMode = GetDefaultBlendColor(),
            };

            fields.Add(fieldRecord);

            return fieldRecord;
        }
    }
}
