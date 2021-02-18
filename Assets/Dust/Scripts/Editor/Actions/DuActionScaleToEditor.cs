using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuActionScaleTo))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuActionScaleToEditor : DuIntervalActionEditor
    {
        private DuProperty m_ScaleTo;

        //--------------------------------------------------------------------------------------------------------------

        static DuActionScaleToEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuActionScaleTo), "ScaleTo");
        }

        [MenuItem("Dust/Actions/ScaleTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Action ScaleTo", typeof(DuActionScaleTo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ScaleTo = FindProperty("m_ScaleTo", "Scale To");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuActionScaleTo.Parameters"))
            {
                PropertyField(m_ScaleTo);
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuActionScaleTo");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
