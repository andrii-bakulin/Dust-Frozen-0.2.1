using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuActionMoveBy))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuActionMoveByEditor : DuIntervalActionEditor
    {
        private DuProperty m_Distance;
        private DuProperty m_DirectionSpace;

        //--------------------------------------------------------------------------------------------------------------

        static DuActionMoveByEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuActionMoveBy), "MoveBy");
        }

        [MenuItem("Dust/Actions/MoveBy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Action MoveBy", typeof(DuActionMoveBy));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Distance = FindProperty("m_Distance", "Distance");
            m_DirectionSpace = FindProperty("m_DirectionSpace", "Direction Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuActionMoveBy.Parameters"))
            {
                PropertyField(m_Distance);
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
                PropertyField(m_DirectionSpace);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuActionMoveBy");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
