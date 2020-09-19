using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public abstract class DuDeformerEditor : DuEditor
    {
        protected DuProperty m_GizmosVisibility;

        protected DuFieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnableDeformer()
        {
            m_GizmosVisibility = FindProperty("m_GizmosVisibility", "Visibility");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as DuDeformer).fieldsMap);
        }

        public override void OnInspectorGUI()
        {
            (target as DuMonoBehaviour).enabled = true; // Forced activate all deformers-scripts
        }

        protected void OnInspectorGUI_FieldsMap()
        {
            m_FieldsMapEditor.OnInspectorGUI();
        }

        protected void OnInspectorGUI_GizmosBlock()
        {
            if (DustGUI.FoldoutBegin("Gizmos", "DuDeformer.Gizmos"))
            {
                PropertyField(m_GizmosVisibility);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
#endif
