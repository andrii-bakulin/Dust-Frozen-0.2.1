using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuMoveByAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuMoveByActionEditor : DuIntervalActionEditor
    {
        private DuProperty m_MoveBy;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static DuMoveByActionEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuMoveByAction), "MoveBy");
        }

        [MenuItem("Dust/Actions/MoveBy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("MoveBy Action", typeof(DuMoveByAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_MoveBy = FindProperty("m_MoveBy", "Move By");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuMoveByAction.Parameters"))
            {
                PropertyField(m_MoveBy);
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
                PropertyField(m_Space);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuMoveByAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
