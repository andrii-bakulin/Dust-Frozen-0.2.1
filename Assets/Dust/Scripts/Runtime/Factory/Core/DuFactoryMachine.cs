using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuFactoryMachine : DuMonoBehaviour
    {
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
        protected float m_Strength = 1.0f;
        public float strength
        {
            get => m_Strength;
            set => m_Strength = value;
        }

        [SerializeField]
        protected DuFieldsMap m_FieldsMap = DuFieldsMap.Factory();
        public DuFieldsMap fieldsMap => m_FieldsMap;

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

        public abstract string FactoryMachineName();

#if UNITY_EDITOR
        public abstract string FactoryMachineDynamicHint();
#endif

        public abstract void PrepareForUpdateInstancesStates(DuFactory factory);
        public abstract void FinalizeUpdateInstancesStates(DuFactory factory);

        public abstract void UpdateInstanceState(DuFactory factory, DuFactoryInstance factoryInstance, float intensityByFactory);
    }
}
