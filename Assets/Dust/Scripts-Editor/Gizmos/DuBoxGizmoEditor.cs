using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuBoxGizmo)), CanEditMultipleObjects]
    public class DuBoxGizmoEditor : DuGizmoEditor
    {
        private DuProperty m_Size;
        private DuProperty m_Center;

        void OnEnable()
        {
            OnEnableGizmo();

            m_Size = FindProperty("m_Size", "Size");
            m_Center = FindProperty("m_Center", "Center");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_Size);
            PropertyField(m_Center);
            Space();
            PropertyField(m_Color);
            PropertyField(m_GizmosVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
