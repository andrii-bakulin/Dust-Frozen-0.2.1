using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuSphereGizmo)), CanEditMultipleObjects]
    public class DuSphereGizmoEditor : DuGizmoEditor
    {
        private DuProperty m_Radius;
        private DuProperty m_Center;

        void OnEnable()
        {
            OnEnableGizmo();

            m_Radius = FindProperty("m_Radius", "Radius");
            m_Center = FindProperty("m_Center", "Center");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyExtendedSlider(m_Radius, 0f, 10f, 0.01f);
            PropertyField(m_Center);
            Space();
            PropertyField(m_Color);
            PropertyField(m_GizmoVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
