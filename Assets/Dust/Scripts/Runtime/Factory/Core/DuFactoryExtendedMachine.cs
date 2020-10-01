using UnityEngine;

namespace DustEngine
{
    public abstract class DuFactoryExtendedMachine : DuFactoryMachine
    {
        protected class MachineParams
        {
            // In
            public DuFactory factory;
            public DuFactoryInstance factoryInstance;
            public float intensityByFactory;
            public float intensityByMachine;

            // Out
            public float fieldPower;
            public Color fieldColor;

            // Supported params:                            // Use by FactoryMachine: Random
            public bool extraIntensityEnabled;
            public Vector3 extraIntensityPosition;
            public Vector3 extraIntensityRotation;
            public Vector3 extraIntensityScale;

            /* @todo!
            public Vector3 offset;                          // Use by FactoryMachine: Random
            public Color color;                             // Use by FactoryMachine: Random, Shader
            */
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /**
         * Calculation steps:
         *     1. calculate new value
         *     2. RESULT multiply for Intensity(s)
         * for Relative:
         *     3. RESULT multiply for fieldPower
         *     4. RESULT added to current value
         * for Absolute:
         *     3. Calculate END-RESULT value as Lerp(current, RESULT, fieldPower)
         */
        public enum TransformMode
        {
            Relative = 0,
            Absolute = 1,
        }

        /**
         * Currently this value works only with position
         */
        public enum TransformSpace
        {
            Factory = 0,
            Instance = 1,
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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
        protected float m_Min = 0.0f;
        public float min
        {
            get => m_Min;
            set => m_Min = value;
        }

        [SerializeField]
        protected float m_Max = 1.0f;
        public float max
        {
            get => m_Max;
            set => m_Max = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected TransformMode m_TransformMode = TransformMode.Relative;
        public TransformMode transformMode
        {
            get => m_TransformMode;
            set => m_TransformMode = value;
        }

        [SerializeField]
        protected TransformSpace m_TransformSpace = TransformSpace.Factory;
        public TransformSpace transformSpace
        {
            get => m_TransformSpace;
            set => m_TransformSpace = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected bool m_PositionEnabled = true;
        public bool positionEnabled
        {
            get => m_PositionEnabled;
            set => m_PositionEnabled = value;
        }

        [SerializeField]
        protected Vector3 m_Position = Vector3.up;
        public Vector3 position
        {
            get => m_Position;
            set => m_Position = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected bool m_RotationEnabled = false;
        public bool rotationEnabled
        {
            get => m_RotationEnabled;
            set => m_RotationEnabled = value;
        }

        [SerializeField]
        protected Vector3 m_Rotation = new Vector3(0f, 90f, 0f);
        public Vector3 rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected bool m_ScaleEnabled = false;
        public bool scaleEnabled
        {
            get => m_ScaleEnabled;
            set => m_ScaleEnabled = value;
        }

        [SerializeField]
        protected Vector3 m_Scale = Vector3.one;
        public Vector3 scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

        [SerializeField]
        protected DebugColorView m_DebugColorView = DebugColorView.Normal;
        public DebugColorView debugColorView
        {
            get => m_DebugColorView;
            set => m_DebugColorView = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected abstract float GetFactoryMachinePower(MachineParams machineParams);
        protected abstract Color GetFactoryMachineColor(MachineParams machineParams);

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void UpdateInstanceDynamicState(DuFactoryInstance.State instanceState, MachineParams machineParams)
        {
            fieldsMap.Calculate(machineParams.factory, machineParams.factoryInstance, out machineParams.fieldPower, out machineParams.fieldColor);

            UpdateInstanceDynamicState_Position(instanceState, machineParams);
            UpdateInstanceDynamicState_Rotation(instanceState, machineParams);
            UpdateInstanceDynamicState_Scale(instanceState, machineParams);

            UpdateInstanceDynamicState_Value(instanceState, machineParams);
            UpdateInstanceDynamicState_Color(instanceState, machineParams);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected virtual void UpdateInstanceDynamicState_Position(DuFactoryInstance.State instanceState, MachineParams machineParams)
        {
            if (!positionEnabled)
                return;

            Vector3 updateForValue = position;
            updateForValue *= machineParams.intensityByFactory * machineParams.intensityByMachine;

            if (machineParams.extraIntensityEnabled)
                updateForValue.Scale(machineParams.extraIntensityPosition);

            switch (transformSpace)
            {
                case TransformSpace.Factory:
                {
                    // Work same for both TransformModes

                    updateForValue = DuMath.RotatePoint(updateForValue, instanceState.rotation);

                    instanceState.position += updateForValue * machineParams.fieldPower;
                    break;
                }

                case TransformSpace.Instance:
                {
                    switch (transformMode)
                    {
                        case TransformMode.Relative:
                            instanceState.position += updateForValue * machineParams.fieldPower;
                            break;

                        case TransformMode.Absolute:
                            instanceState.position = Vector3.LerpUnclamped(instanceState.position, updateForValue, machineParams.fieldPower);
                            break;
                    }

                    break;
                }
            }
        }

        protected virtual void UpdateInstanceDynamicState_Rotation(DuFactoryInstance.State instanceState, MachineParams machineParams)
        {
            if (!rotationEnabled)
                return;

            Vector3 updateForValue = rotation;
            updateForValue *= machineParams.intensityByFactory * machineParams.intensityByMachine;

            if (machineParams.extraIntensityEnabled)
                updateForValue.Scale(machineParams.extraIntensityRotation);

            switch (transformMode)
            {
                case TransformMode.Relative:
                    instanceState.rotation += updateForValue * machineParams.fieldPower;
                    break;

                case TransformMode.Absolute:
                    instanceState.rotation = Vector3.LerpUnclamped(instanceState.rotation, updateForValue, machineParams.fieldPower);
                    break;
            }
        }

        protected virtual void UpdateInstanceDynamicState_Scale(DuFactoryInstance.State instanceState, MachineParams machineParams)
        {
            if (!scaleEnabled)
                return;

            Vector3 endScale = scale * (machineParams.intensityByFactory * machineParams.intensityByMachine);

            if (machineParams.extraIntensityEnabled)
                endScale.Scale(machineParams.extraIntensityScale);

            // Notice: if instanceState.scale is 2.0f and I need scale relative +1.0f
            // then result should be 4.0f (not 3.0f)
            // So here require recalculate updateForValue bases on current object scale
            // And later apply field-power
            Vector3 newRelativeValue = Vector3.Scale(instanceState.scale, endScale);

            switch (transformMode)
            {
                case TransformMode.Relative:
                    instanceState.scale += newRelativeValue * machineParams.fieldPower;
                    break;

                case TransformMode.Absolute:
                    instanceState.scale = Vector3.LerpUnclamped(instanceState.scale, newRelativeValue, machineParams.fieldPower);
                    break;
            }
        }

        protected virtual void UpdateInstanceDynamicState_Value(DuFactoryInstance.State instanceState, MachineParams machineParams)
        {
            if (DuMath.IsZero(valueImpactIntensity))
                return;

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
                    newValue = GetFactoryMachinePower(machineParams);
                    break;

                case ValueImpactSource.FieldsMapPower:
                    newValue = machineParams.fieldPower;
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

            instanceState.value = Mathf.LerpUnclamped(instanceState.value, newValue, machineParams.intensityByFactory * valueImpactIntensity);

            if (valueClampEnabled)
                instanceState.value = Mathf.Clamp(instanceState.value, valueClampMin, valueClampMax);
        }

        protected virtual void UpdateInstanceDynamicState_Color(DuFactoryInstance.State instanceState, MachineParams machineParams)
        {
            if (DuMath.IsZero(colorImpactIntensity))
                return;

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
                    newColor = GetFactoryMachineColor(machineParams);
                    break;

                case ColorImpactSource.FieldsMapColor:
                    newColor = machineParams.fieldColor;
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

            float colorPower = machineParams.fieldPower;

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

            instanceState.color = Color.LerpUnclamped(instanceState.color, newColor, Mathf.Abs(machineParams.intensityByFactory) * colorPower * colorImpactIntensity);
            instanceState.color.Clamp01();
        }
    }
}
