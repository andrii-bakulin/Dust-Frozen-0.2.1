﻿using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuFactoryMachine : DuMonoBehaviour
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

            // Calculated-n-Supported params:               // Use by FactoryMachine: Random
            public bool extraIntensityEnabled;
            public Vector3 extraIntensityPosition;
            public Vector3 extraIntensityRotation;
            public Vector3 extraIntensityScale;

            /* @todo!
            public Vector3 offset;                          // Use by FactoryMachine: Random
            public Color color;                             // Use by FactoryMachine: Random, Shader
            */
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
        protected float m_Intensity = 1.0f;
        public float intensity
        {
            get => m_Intensity;
            set => m_Intensity = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        public static void AddFactoryMachineComponentByType(System.Type factoryType)
        {
            Selection.activeGameObject = AddFactoryMachineComponentByType(Selection.activeGameObject, factoryType);
        }

        public static GameObject AddFactoryMachineComponentByType(GameObject activeGameObject, System.Type factoryMachineType)
        {
            DuFactory selectedFactory = null;

            if (Dust.IsNotNull(activeGameObject))
            {
                selectedFactory = activeGameObject.GetComponent<DuFactory>();

                if (Dust.IsNull(selectedFactory) && Dust.IsNotNull(activeGameObject.transform.parent))
                    selectedFactory = activeGameObject.transform.parent.GetComponent<DuFactory>();
            }

            var gameObject = new GameObject();
            {
                var factoryMachine = gameObject.AddComponent(factoryMachineType) as DuFactoryMachine;

                if (Dust.IsNotNull(selectedFactory))
                {
                    selectedFactory.AddFactoryMachine(factoryMachine);
                }

                gameObject.name = factoryMachine.FactoryMachineName() + " Machine";
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }

            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            return gameObject;
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            // Require to show enabled-checkbox in editor for all factory-machine
        }

        //--------------------------------------------------------------------------------------------------------------

        public abstract string FactoryMachineName();

#if UNITY_EDITOR
        public virtual string FactoryMachineDynamicHint()
        {
            return "";
        }
#endif

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public abstract bool PrepareForUpdateInstancesStates(FactoryInstanceState factoryInstanceState);

        public abstract void UpdateInstanceState(FactoryInstanceState factoryInstanceState);

        public abstract void FinalizeUpdateInstancesStates(FactoryInstanceState factoryInstanceState);
    }
}