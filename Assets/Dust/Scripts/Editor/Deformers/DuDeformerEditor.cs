using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuDeformerEditor : DuEditor
    {
        protected DuProperty m_GizmoVisibility;

        protected DuFieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnableDeformer()
        {
            m_GizmoVisibility = FindProperty("m_GizmoVisibility", "Visibility");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as DuDeformer).fieldsMap);
        }

        public override void OnInspectorGUI()
        {
            // Hide default OnInspectorGUI() call
            // Extend all-deformers-view if need in future...
        }

        protected void OnInspectorGUI_FieldsMap()
        {
            m_FieldsMapEditor.OnInspectorGUI();
        }

        protected void OnInspectorGUI_GizmoBlock()
        {
            if (DustGUI.FoldoutBegin("Gizmo", "DuDeformer.Gizmo"))
            {
                PropertyField(m_GizmoVisibility);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
