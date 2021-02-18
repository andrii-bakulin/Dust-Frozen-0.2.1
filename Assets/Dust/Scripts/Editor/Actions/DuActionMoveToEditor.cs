using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuActionMoveTo))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuActionMoveToEditor : DuIntervalActionEditor
    {
        private DuProperty m_EndPosition;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static DuActionMoveToEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuActionMoveTo), "MoveTo");
        }

        [MenuItem("Dust/Actions/MoveTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Action MoveTo", typeof(DuActionMoveTo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_EndPosition = FindProperty("m_EndPosition", "Move To");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuActionMoveTo.Parameters"))
            {
                PropertyField(m_EndPosition);
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
                PropertyField(m_Space);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuActionMoveTo");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
