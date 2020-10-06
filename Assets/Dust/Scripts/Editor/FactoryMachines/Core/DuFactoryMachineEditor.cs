using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuFactoryMachineEditor : DuEditor
    {
        protected DuProperty m_CustomHint;
        protected DuProperty m_Intensity;

        //--------------------------------------------------------------------------------------------------------------

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

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnableFactoryMachine()
        {
            m_CustomHint = FindProperty("m_CustomHint", "Hint for Machine");
            m_Intensity = FindProperty("m_Intensity", "Intensity");
        }

        public override void OnInspectorGUI()
        {
            // Hide default OnInspectorGUI() call
            // Extend all-factoryMachines-view if need in future...
        }

        protected virtual void OnInspectorGUI_BaseParameters()
        {
            if (DustGUI.FoldoutBegin("Parameters", "DuFactoryMachine.Parameters"))
            {
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();

                PropertyField(m_CustomHint);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
