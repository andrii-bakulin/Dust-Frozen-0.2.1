using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTransformFactoryMachine))]
    [CanEditMultipleObjects]
    public class DuTransformFactoryMachineEditor : DuPRSFactoryMachineEditor
    {
        void OnEnable()
        {
            OnEnableFactoryMachine();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseParameters();

            OnInspectorGUI_TransformBlock();

            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();
            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
