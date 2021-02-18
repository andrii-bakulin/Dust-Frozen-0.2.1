using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuActionScaleBy))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuActionScaleByEditor : DuIntervalActionEditor
    {
        private DuProperty m_ScaleBy;

        //--------------------------------------------------------------------------------------------------------------

        static DuActionScaleByEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuActionScaleBy), "ScaleBy");
        }

        [MenuItem("Dust/Actions/ScaleBy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Action ScaleBy", typeof(DuActionScaleBy));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ScaleBy = FindProperty("m_ScaleBy", "Scale By");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuActionScaleBy.Parameters"))
            {
                PropertyField(m_ScaleBy);
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuActionScaleBy");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
