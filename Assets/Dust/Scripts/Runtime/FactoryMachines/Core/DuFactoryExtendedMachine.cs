using UnityEngine;

namespace DustEngine
{
    public abstract class DuFactoryExtendedMachine : DuFactoryMachine
    {
        public enum ValueImpactSource
        {
            Skip = 0,
            FixedValue = 1,
            FactoryMachinePower = 2,
            FieldsMapPower = 3,
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
            Skip = 0,
            FixedColor = 1,
            FactoryMachineColor = 2,
            FieldsMapColor = 3,
        }

        public enum ColorBlendMode
        {
            Set = 0,
            Add = 1,
            Subtract = 2,
            Multiply = 3,
            Divide = 4,
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public enum DebugColorView
        {
            Normal = 0,
            FalloffColor = 1,
            FalloffAlpha = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected ValueImpactSource m_ValueImpactSource = ValueImpactSource.Skip;
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
        protected ColorImpactSource m_ColorImpactSource = ColorImpactSource.Skip;
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
        protected ColorBlendMode m_ColorBlendMode = ColorBlendMode.Set;
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
        protected DuFieldsMap m_FieldsMap = DuFieldsMap.Factory();
        public DuFieldsMap fieldsMap => m_FieldsMap;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // @todo! @todo@ >> drop?
        [SerializeField]
        protected DebugColorView m_DebugColorView = DebugColorView.Normal;
        public DebugColorView debugColorView
        {
            get => m_DebugColorView;
            set => m_DebugColorView = value;
        }

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
            if (DuMath.IsZero(valueImpactIntensity))
                return;

            var instanceState = factoryInstanceState.instance.stateDynamic;

            float newValue;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Calculate value

            switch (valueImpactSource)
            {
                default:
                case ValueImpactSource.Skip:
                    return;

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

            instanceState.value = Mathf.LerpUnclamped(instanceState.value, newValue, factoryInstanceState.intensityByFactory * valueImpactIntensity);

            if (valueClampEnabled)
                instanceState.value = Mathf.Clamp(instanceState.value, valueClampMin, valueClampMax);
        }

        protected void UpdateInstanceDynamicState_Color(FactoryInstanceState factoryInstanceState)
        {
            if (DuMath.IsZero(colorImpactIntensity))
                return;

            var instanceState = factoryInstanceState.instance.stateDynamic;

            Color newColor;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Calculate color

            switch (colorImpactSource)
            {
                case ColorImpactSource.Skip:
                default:
                    return;

                case ColorImpactSource.FixedColor:
                    newColor = colorFixed;
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

            switch (colorBlendMode)
            {
                case ColorBlendMode.Set:
                default:
                    // Nothing need to do
                    break;

                case ColorBlendMode.Add:
                    newColor = DuColor.AddRGB(instanceState.color, newColor.ToRGBWithoutAlpha());
                    break;

                case ColorBlendMode.Subtract:
                    newColor = DuColor.SubtractRGB(instanceState.color, newColor.ToRGBWithoutAlpha());
                    break;

                case ColorBlendMode.Multiply:
                    newColor = instanceState.color * newColor;
                    newColor.Clamp01();
                    break;

                case ColorBlendMode.Divide:
                    newColor = DuColor.DivideSafely(instanceState.color, newColor);
                    newColor.Clamp01();
                    break;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Merge

            float colorPower = factoryInstanceState.fieldPower;

            if (colorImpactSource == ColorImpactSource.FieldsMapColor)
            {
                colorPower = newColor.a;
                newColor.a = 1f;
            }

#if UNITY_EDITOR
            switch (debugColorView)
            {
                default:
                case DebugColorView.Normal:
                    // use down code...
                    break;

                case DebugColorView.FalloffColor:
                    instanceState.color = newColor * colorPower;
                    instanceState.color.Clamp01();
                    return;

                case DebugColorView.FalloffAlpha:
                    instanceState.color = new Color(colorPower, colorPower, colorPower);
                    instanceState.color.Clamp01();
                    return;
            }
#endif

            instanceState.color = Color.LerpUnclamped(instanceState.color, newColor,
                Mathf.Abs(factoryInstanceState.intensityByFactory) * colorPower * colorImpactIntensity);
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
