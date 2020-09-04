using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public abstract class DuGizmoEditor : DuEditor
    {
        protected DuProperty m_Color;
        protected DuProperty m_GizmosVisibility;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnEnableGizmo()
        {
            m_Color = FindProperty("m_Color", "Color");
            m_GizmosVisibility = FindProperty("m_GizmosVisibility", "Visibility");
        }

        public override void OnInspectorGUI()
        {
        }
    }
}
#endif
