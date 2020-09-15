using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public abstract class DuObjectFieldEditor : DuFieldEditor
    {
        protected DuRemappingEditor m_RemappingEditor;

        protected DuProperty m_GizmoVisibility;
        protected DuProperty m_GizmoFieldColor;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnableField()
        {
            base.OnEnableField();

            m_RemappingEditor = new DuRemappingEditor((target as DuObjectField).remapping, serializedObject.FindProperty("m_Remapping"));

            m_GizmoVisibility = FindProperty("m_GizmoVisibility", "Visibility");
            m_GizmoFieldColor = FindProperty("m_GizmoFieldColor", "Use Field Color");
        }

        protected void OnInspectorGUI_RemappingBlock()
        {
            m_RemappingEditor.OnInspectorGUI(false);
        }

        protected void OnInspectorGUI_GizmosBlock()
        {
            if (DustGUI.FoldoutBegin("Gizmos", "DuField.Gizmos"))
            {
                PropertyField(m_GizmoVisibility);
                PropertyField(m_GizmoFieldColor);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
#endif
