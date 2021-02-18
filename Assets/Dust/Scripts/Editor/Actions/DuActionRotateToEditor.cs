using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuActionRotateTo))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuActionRotateToEditor : DuIntervalActionEditor
    {
        private DuProperty m_RotateTo;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static DuActionRotateToEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuActionRotateTo), "RotateTo");
        }

        [MenuItem("Dust/Actions/RotateTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Action RotateTo", typeof(DuActionRotateTo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_RotateTo = FindProperty("m_RotateTo", "Rotate To");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuActionRotateTo.Parameters"))
            {
                PropertyField(m_RotateTo);
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
                PropertyField(m_Space);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuActionRotateTo");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
