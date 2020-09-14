using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public abstract class DuObjectFieldEditor : DuFieldEditor
    {
        protected DuRemappingEditor m_RemappingEditor;

        protected DuProperty m_GizmosVisibility;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnableField()
        {
            base.OnEnableField();

            m_RemappingEditor = new DuRemappingEditor((target as DuObjectField).remapping, serializedObject.FindProperty("m_Remapping"));

            m_GizmosVisibility = FindProperty("m_GizmosVisibility", "Visibility");
        }

        protected void OnInspectorGUI_RemappingBlock()
        {
            m_RemappingEditor.OnInspectorGUI(false);
        }

        protected void OnInspectorGUI_GizmosBlock()
        {
            if (DustGUI.FoldoutBegin("Gizmos", "DuField.Gizmos"))
            {
                PropertyField(m_GizmosVisibility);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
#endif
