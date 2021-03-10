using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuMoveByAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuMoveByActionEditor : DuIntervalWithRollbackActionEditor
    {
        private DuProperty m_MoveBy;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static DuMoveByActionEditor()
        {
            DuActionsPopupButtons.AddActionAnimate(typeof(DuMoveByAction), "MoveBy");
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
                OnInspectorGUI_Duration();
                PropertyField(m_Space);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuMoveByAction");
            OnInspectorGUI_Extended("DuMoveByAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
