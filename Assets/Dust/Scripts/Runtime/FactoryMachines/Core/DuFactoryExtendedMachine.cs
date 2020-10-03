using UnityEngine;

namespace DustEngine
{
    public abstract class DuFactoryExtendedMachine : DuFactoryMachine
    {
        public enum ValueImpactSource
        {
            FieldsMapPower = 0,
            FactoryMachinePower = 1,
            FixedValue = 2,
        }

        public enum ValueBlendMode
        {
            Set = 0,
            Add = 1,
            Subtract = 2,
            Multiply = 3,
            Divide = 4,
            Avg = 5,
            Min = 6,
            Max = 7,
            BlendClamped = 8,
            BlendUnclamped = 9,
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public enum ColorImpactSource
        {
            FieldsMapColor = 0,
            FactoryMachineColor = 1,
            FixedColor = 2,
        }

        public enum ColorBlendMode
        {
            Blend = 0,
            Set = 1,
            Add = 2,
            Subtract = 3,
            Multiply = 4,
            Min = 5,
            Max = 6,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected bool m_ValueImpactEnabled = true;
        public bool valueImpactEnabled
        {
            get => m_ValueImpactEnabled;
            set => m_ValueImpactEnabled = value;
        }

        [SerializeField]
        protected ValueImpactSource m_ValueImpactSource = ValueImpactSource.FieldsMapPower;
        public ValueImpactSource valueImpactSource
        {
            get => m_ValueImpactSource;
            set => m_ValueImpactSource = value;
        }

        [SerializeField]
        protected float m_ValueImpactIntensity = 1f;
        public float valueImpactIntensity
        {
            get => m_ValueImpactIntensity;
            set => m_ValueImpactIntensity = value;
        }

        [SerializeField]
        protected ValueBlendMode m_ValueBlendMode = ValueBlendMode.Set;
        public ValueBlendMode valueBlendMode
        {
            get => m_ValueBlendMode;
            set => m_ValueBlendMode = value;
        }

        [SerializeField]
        protected float m_ValueFixed = 0f;
        public float valueFixed
        {
            get => m_ValueFixed;
            set => m_ValueFixed = value;
        }

        [SerializeField]
        protected bool m_ValueClampEnabled = false;
        public bool valueClampEnabled
        {
            get => m_ValueClampEnabled;
            set => m_ValueClampEnabled = value;
        }

        [SerializeField]
        protected float m_ValueClampMin = 0f;
        public float valueClampMin
        {
            get => m_ValueClampMin;
            set => m_ValueClampMin = value;
        }

        [SerializeField]
        protected float m_ValueClampMax = 1f;
        public float valueClampMax
        {
            get => m_ValueClampMax;
            set => m_ValueClampMax = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected bool m_ColorImpactEnabled = true;
        public bool colorImpactEnabled
        {
            get => m_ColorImpactEnabled;
            set => m_ColorImpactEnabled = value;
        }

        [SerializeField]
        protected ColorImpactSource m_ColorImpactSource = ColorImpactSource.FieldsMapColor;
        public ColorImpactSource colorImpactSource
        {
            get => m_ColorImpactSource;
            set => m_ColorImpactSource = value;
        }

        [SerializeField]
        protected float m_ColorImpactIntensity = 1f;
        public float colorImpactIntensity
        {
            get => m_ColorImpactIntensity;
            set => m_ColorImpactIntensity = value;
        }

        [SerializeField]
        protected ColorBlendMode m_ColorBlendMode = ColorBlendMode.Blend;
        public ColorBlendMode colorBlendMode
        {
            get => m_ColorBlendMode;
            set => m_ColorBlendMode = value;
        }

        [SerializeField]
        protected Color m_ColorFixed = Color.white;
        public Color colorFixed
        {
            get => m_ColorFixed;
            set => m_ColorFixed = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected DuFieldsMap m_FieldsMap = DuFieldsMap.FactoryMachine();
        public DuFieldsMap fieldsMap => m_FieldsMap;

        //--------------------------------------------------------------------------------------------------------------

        public override bool PrepareForUpdateInstancesStates(FactoryInstanceState factoryInstanceState)
        {
            return DuMath.IsNotZero(factoryInstanceState.intensityByFactory);
        }

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);
        }

        public override void FinalizeUpdateInstancesStates(FactoryInstanceState factoryInstanceState)
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void UpdateInstanceDynamicState(FactoryInstanceState factoryInstanceState, float intensityByMachine)
        {
            factoryInstanceState.intensityByMachine = intensityByMachine;

            fieldsMap.Calculate(factoryInstanceState.factory, factoryInstanceState.instance,
                out factoryInstanceState.fieldPower,
                out factoryInstanceState.fieldColor);

            UpdateInstanceDynamicState_Value(factoryInstanceState);
            UpdateInstanceDynamicState_Color(factoryInstanceState);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected void UpdateInstanceDynamicState_Value(FactoryInstanceState factoryInstanceState)
        {
            if (!valueImpactEnabled)
                return;

            if (DuMath.IsZero(valueImpactIntensity) || DuMath.IsZero(factoryInstanceState.intensityByMachine))
                return;

            var instanceState = factoryInstanceState.instance.stateDynamic;

            float newValue;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Calculate value

            switch (valueImpactSource)
            {
                default:
                case ValueImpactSource.FixedValue:
                    newValue = valueFixed;
                    break;

                case ValueImpactSource.FactoryMachinePower:
                    newValue = GetFactoryMachinePower(factoryInstanceState);
                    break;

                case ValueImpactSource.FieldsMapPower:
                    newValue = factoryInstanceState.fieldPower;
                    break;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Blending

            switch (valueBlendMode)
            {
                default:
                case ValueBlendMode.Set:
                    break;

                case ValueBlendMode.Add:
                    newValue = instanceState.value + newValue;
                    break;

                case ValueBlendMode.Subtract:
                    newValue = instanceState.value - newValue;
                    break;

                case ValueBlendMode.Multiply:
                    newValue = instanceState.value * newValue;
                    break;

                case ValueBlendMode.Divide:
                    if (newValue > 0f)
                        newValue = instanceState.value / newValue;
                    else
                        newValue = 0f;
                    break;

                case ValueBlendMode.Avg:
                    newValue = (instanceState.value + newValue) / 2f;
                    break;

                case ValueBlendMode.Min:
                    newValue = Mathf.Min(instanceState.value, newValue);
                    break;

                case ValueBlendMode.Max:
                    newValue = Mathf.Max(instanceState.value, newValue);
                    break;

                case ValueBlendMode.BlendClamped:
                    newValue = Mathf.Lerp(instanceState.value, newValue, newValue);
                    break;

                case ValueBlendMode.BlendUnclamped:
                    newValue = Mathf.LerpUnclamped(instanceState.value, newValue, newValue);
                    break;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Merge

            float finalIntensity = factoryInstanceState.intensityByFactory
                                   * factoryInstanceState.intensityByMachine
                                   * valueImpactIntensity;

            instanceState.value = Mathf.LerpUnclamped(instanceState.value, newValue, finalIntensity);

            if (valueClampEnabled)
                instanceState.value = Mathf.Clamp(instanceState.value, valueClampMin, valueClampMax);
        }

        protected void UpdateInstanceDynamicState_Color(FactoryInstanceState factoryInstanceState)
        {
            if (!colorImpactEnabled)
                return;

            if (DuMath.IsZero(colorImpactIntensity) || DuMath.IsZero(factoryInstanceState.intensityByMachine))
                return;

            var instanceState = factoryInstanceState.instance.stateDynamic;

            Color newColor;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Calculate color

            switch (colorImpactSource)
            {
                default:
                case ColorImpactSource.FixedColor:
                    newColor = colorFixed;
                    newColor.a = factoryInstanceState.fieldPower;
                    break;

                case ColorImpactSource.FactoryMachineColor:
                    newColor = GetFactoryMachineColor(factoryInstanceState);
                    break;

                case ColorImpactSource.FieldsMapColor:
                    newColor = factoryInstanceState.fieldColor;
                    break;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Blending

            float finalIntensity = Mathf.Abs(factoryInstanceState.intensityByFactory)
                                   * factoryInstanceState.intensityByMachine
                                   * colorImpactIntensity;

            switch (colorBlendMode)
            {
                case ColorBlendMode.Blend:
                default:
                    newColor = DuColorBlend.AlphaBlend(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Set:
                    // nothing need to do
                    // newColor = newColor;
                    break;

                case ColorBlendMode.Add:
                    newColor = DuColorBlend.Add(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Subtract:
                    newColor = DuColorBlend.Subtract(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Multiply:
                    newColor = DuColorBlend.Multiply(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Min:
                    newColor = DuColorBlend.MinAfterBlend(instanceState.color, newColor);
                    break;

                case ColorBlendMode.Max:
                    newColor = DuColorBlend.MaxAfterBlend(instanceState.color,newColor);
                    break;
            }

            instanceState.color = Color.LerpUnclamped(instanceState.color, newColor, finalIntensity);
            instanceState.color.Clamp01();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual float GetFactoryMachinePower(FactoryInstanceState factoryInstanceState)
        {
            return factoryInstanceState.intensityByMachine;
        }

        protected virtual Color GetFactoryMachineColor(FactoryInstanceState factoryInstanceState)
        {
            return Color.white;
        }
    }
}
