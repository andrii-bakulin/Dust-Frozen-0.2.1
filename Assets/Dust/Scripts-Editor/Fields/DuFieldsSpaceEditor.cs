using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFieldsSpace))]
    public class DuFieldsSpaceEditor : DuEditor
    {
        protected DuProperty m_GizmosVisibility;

        protected DuFieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        protected void OnEnable()
        {
            m_GizmosVisibility = FindProperty("m_GizmosVisibility", "Visibility");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as DuFieldsSpace).fieldsMap);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_FieldsMapEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
