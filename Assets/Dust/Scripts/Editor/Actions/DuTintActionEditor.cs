using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTintAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTintActionEditor : DuIntervalWithRollbackActionEditor
    {
        private DuProperty m_TintColor;
        private DuProperty m_TintMode;
        private DuProperty m_PropertyName;

        protected DuTintAction.TintMode tintMode => (DuTintAction.TintMode) m_TintMode.valInt;

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
            m_TintMode = FindProperty("m_TintMode", "Tint Mode");
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
                PropertyField(m_TintMode);
                
                if (tintMode == DuTintAction.TintMode.MeshRenderer)
                {
                    PropertyField(m_PropertyName);
                }
                
                Space();
                
                OnInspectorGUI_Duration();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuTintAction");
            OnInspectorGUI_Extended("DuTintAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
