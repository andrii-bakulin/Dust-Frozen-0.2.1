using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRemapField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuRemapFieldEditor : DuFieldEditor
    {
        protected DuRemappingEditor m_RemappingEditor;

        static DuRemapFieldEditor()
        {
            DuPopupButtons.AddMathField(typeof(DuRemapField), "Remap");
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_RemappingEditor = new DuRemappingEditor((target as DuRemapField).remapping, serializedObject.FindProperty("m_Remapping"));
        }

        [MenuItem("Dust/Fields/Math Fields/Remap")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuRemapField));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_RemappingEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
