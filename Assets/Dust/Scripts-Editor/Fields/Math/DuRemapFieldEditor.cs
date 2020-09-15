using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRemapField)), CanEditMultipleObjects]
    public class DuRemapFieldEditor : DuFieldEditor
    {
        protected DuRemappingEditor m_RemappingEditor;

        void OnEnable()
        {
            OnEnableField();

            m_RemappingEditor = new DuRemappingEditor((target as DuRemapField).remapping, serializedObject.FindProperty("m_Remapping"));
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
#endif
