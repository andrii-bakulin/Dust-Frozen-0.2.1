﻿using UnityEngine;

namespace DustEngine
{
    public abstract class DuFactoryMachine : DuMonoBehaviour, DuDynamicStateInterface
    {
        public class FactoryInstanceState
        {
            // In
            public DuFactory factory;
            public DuFactoryInstance instance;
            public float intensityByFactory;
            public float intensityByMachine;

            // Calculated values
            public float fieldPower;
            public Color fieldColor;

            // Calculated-n-Supported params:               // Use by FactoryMachine: Noise
            public bool extraPowerEnabled;
            public Vector3 extraPowerPosition;
            public Vector3 extraPowerRotation;
            public Vector3 extraPowerScale;
        }

        [System.Serializable]
        public class Record
        {
            [SerializeField]
            private DuFactoryMachine m_FactoryMachine = null;
            public DuFactoryMachine factoryMachine
            {
                get => m_FactoryMachine;
                set => m_FactoryMachine = value;
            }

            [SerializeField]
            private float m_Intensity = 1f;
            public float intensity
            {
                get => m_Intensity;
                set => m_Intensity = value;
            }

            [SerializeField]
            private bool m_Enabled = true;
            public bool enabled
            {
                get => m_Enabled;
                set => m_Enabled = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected string m_CustomHint = "";
        public string customHint
        {
            get => m_CustomHint;
            set => m_CustomHint = value;
        }

        [SerializeField]
        protected float m_Intensity = 1.0f;
        public float intensity
        {
            get => m_Intensity;
            set => m_Intensity = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            // Require to show enabled-checkbox in editor for all factory-machine
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public virtual int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, intensity);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public abstract string FactoryMachineName();

        public abstract string FactoryMachineDynamicHint();

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public abstract bool PrepareForUpdateInstancesStates(FactoryInstanceState factoryInstanceState);

        public abstract void UpdateInstanceState(FactoryInstanceState factoryInstanceState);

        public abstract void FinalizeUpdateInstancesStates(FactoryInstanceState factoryInstanceState);
    }
}
