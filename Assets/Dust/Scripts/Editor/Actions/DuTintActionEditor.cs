using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTintAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTintActionEditor : DuIntervalActionEditor
    {
        private DuProperty m_TintColor;
        private DuProperty m_MeshRenderer;
        private DuProperty m_PropertyName;

        //--------------------------------------------------------------------------------------------------------------

        static DuTintActionEditor()
        {
            DuActionsPopupButtons.AddActionAnimate(typeof(DuTintAction), "Tint");
        }

        [MenuItem("Dust/Actions/Tint")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Tint Action", typeof(DuTintAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_TintColor = FindProperty("m_TintColor", "Tint Color");
            m_MeshRenderer = FindProperty("m_MeshRenderer", "Mesh Renderer");
            m_PropertyName = FindProperty("m_PropertyName", "Property Name");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuTintAction.Parameters"))
            {
                PropertyField(m_TintColor);
                PropertyDurationSlider(m_Duration);
                
                Space();
                
                PropertyField(m_MeshRenderer);
                PropertyField(m_PropertyName);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuTintAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
