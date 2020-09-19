using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuStepObjectsField)), CanEditMultipleObjects]
    public class DuStepObjectsFieldEditor : DuFieldEditor
    {
        protected DuRemappingEditor m_RemappingEditor;

        void OnEnable()
        {
            OnEnableField();

            m_RemappingEditor = new DuRemappingEditor((target as DuStepObjectsField).remapping, serializedObject.FindProperty("m_Remapping"));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_RemappingEditor.OnInspectorGUI(false);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
