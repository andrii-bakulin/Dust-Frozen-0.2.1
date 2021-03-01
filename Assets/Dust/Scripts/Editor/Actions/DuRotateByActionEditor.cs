using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRotateByAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuRotateByActionEditor : DuIntervalActionEditor
    {
        private DuProperty m_RotateBy;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static DuRotateByActionEditor()
        {
            DuActionsPopupButtons.AddActionAnimate(typeof(DuRotateByAction), "RotateBy");
        }

        [MenuItem("Dust/Actions/RotateBy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("RotateBy Action", typeof(DuRotateByAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_RotateBy = FindProperty("m_RotateBy", "Rotate By");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuRotateByAction.Parameters"))
            {
                PropertyField(m_RotateBy);
                PropertyDurationSlider(m_Duration);
                PropertyField(m_Space);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuRotateByAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
