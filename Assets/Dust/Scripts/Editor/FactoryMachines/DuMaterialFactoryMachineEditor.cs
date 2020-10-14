using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuMaterialFactoryMachine))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuMaterialFactoryMachineEditor : DuFactoryMachineEditor
    {
        //--------------------------------------------------------------------------------------------------------------

        static DuMaterialFactoryMachineEditor()
        {
            DuPopupButtons.AddFactoryMachine(typeof(DuMaterialFactoryMachine), "Material");
        }

        [MenuItem("Dust/Factory/Machines/Material")]
        public static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuMaterialFactoryMachine));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableFactoryMachine();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuFactoryMachine.Parameters"))
            {
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();

                PropertyField(m_CustomHint);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
