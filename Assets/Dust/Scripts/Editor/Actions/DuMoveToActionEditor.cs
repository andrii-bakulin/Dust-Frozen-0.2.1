using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuMoveToAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuMoveToActionEditor : DuIntervalActionEditor
    {
        private DuProperty m_MoveTo;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static DuMoveToActionEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuMoveToAction), "MoveTo");
        }

        [MenuItem("Dust/Actions/MoveTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("MoveTo Action", typeof(DuMoveToAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_MoveTo = FindProperty("m_MoveTo", "Move To");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuMoveToAction.Parameters"))
            {
                PropertyField(m_MoveTo);
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
                PropertyField(m_Space);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuMoveToAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}