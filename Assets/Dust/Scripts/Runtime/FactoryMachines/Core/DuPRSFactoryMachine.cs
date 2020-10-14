using UnityEngine;

namespace DustEngine
{
    public abstract class DuPRSFactoryMachine : DuBasicFactoryMachine
    {
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

        //--------------------------------------------------------------------------------------------------------------

        protected override void UpdateInstanceDynamicState(FactoryInstanceState factoryInstanceState, float intensityByMachine)
        {
            base.UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);

            UpdateInstanceDynamicState_Position(factoryInstanceState);
            UpdateInstanceDynamicState_Rotation(factoryInstanceState);
            UpdateInstanceDynamicState_Scale(factoryInstanceState);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected void UpdateInstanceDynamicState_Position(FactoryInstanceState factoryInstanceState)
        {
            if (!positionEnabled)
                return;

            var instanceState = factoryInstanceState.instance.stateDynamic;

            Vector3 updateForValue = position;
            updateForValue *= factoryInstanceState.intensityByFactory * factoryInstanceState.intensityByMachine;

            if (factoryInstanceState.extraIntensityEnabled)
                updateForValue.Scale(factoryInstanceState.extraIntensityPosition);

            switch (transformSpace)
            {
                case TransformSpace.Factory:
                {
                    // Work same for both TransformModes

                    updateForValue = DuMath.RotatePoint(updateForValue, instanceState.rotation);

                    instanceState.position += updateForValue * factoryInstanceState.fieldPower;
                    break;
                }

                case TransformSpace.Instance:
                {
                    switch (transformMode)
                    {
                        case TransformMode.Relative:
                            instanceState.position += updateForValue * factoryInstanceState.fieldPower;
                            break;

                        case TransformMode.Absolute:
                            instanceState.position = Vector3.LerpUnclamped(instanceState.position, updateForValue, factoryInstanceState.fieldPower);
                            break;
                    }

                    break;
                }
            }
        }

        protected void UpdateInstanceDynamicState_Rotation(FactoryInstanceState factoryInstanceState)
        {
            if (!rotationEnabled)
                return;

            var instanceState = factoryInstanceState.instance.stateDynamic;

            Vector3 updateForValue = rotation;
            updateForValue *= factoryInstanceState.intensityByFactory * factoryInstanceState.intensityByMachine;

            if (factoryInstanceState.extraIntensityEnabled)
                updateForValue.Scale(factoryInstanceState.extraIntensityRotation);

            switch (transformMode)
            {
                case TransformMode.Relative:
                    instanceState.rotation += updateForValue * factoryInstanceState.fieldPower;
                    break;

                case TransformMode.Absolute:
                    instanceState.rotation = Vector3.LerpUnclamped(instanceState.rotation, updateForValue, factoryInstanceState.fieldPower);
                    break;
            }
        }

        protected void UpdateInstanceDynamicState_Scale(FactoryInstanceState factoryInstanceState)
        {
            if (!scaleEnabled)
                return;

            var instanceState = factoryInstanceState.instance.stateDynamic;

            Vector3 endScale = scale * (factoryInstanceState.intensityByFactory * factoryInstanceState.intensityByMachine);

            if (factoryInstanceState.extraIntensityEnabled)
                endScale.Scale(factoryInstanceState.extraIntensityScale);

            // Notice: if instanceState.scale is 2.0f and I need scale relative +1.0f
            // then result should be 4.0f (not 3.0f)
            // So here require recalculate updateForValue bases on current object scale
            // And later apply field-power
            Vector3 newRelativeValue = Vector3.Scale(instanceState.scale, endScale);

            switch (transformMode)
            {
                case TransformMode.Relative:
                    instanceState.scale += newRelativeValue * factoryInstanceState.fieldPower;
                    break;

                case TransformMode.Absolute:
                    instanceState.scale = Vector3.LerpUnclamped(instanceState.scale, newRelativeValue, factoryInstanceState.fieldPower);
                    break;
            }
        }
    }
}
