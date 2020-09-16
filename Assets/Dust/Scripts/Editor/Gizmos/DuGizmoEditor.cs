using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public abstract class DuGizmoEditor : DuEditor
    {
        protected DuProperty m_Color;
        protected DuProperty m_GizmoVisibility;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnableGizmo()
        {
            m_Color = FindProperty("m_Color", "Color");
            m_GizmoVisibility = FindProperty("m_GizmoVisibility", "Visibility");
        }

        public override void OnInspectorGUI()
        {
        }
    }
}
#endif
