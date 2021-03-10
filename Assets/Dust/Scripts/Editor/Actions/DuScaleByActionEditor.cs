using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuScaleByAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuScaleByActionEditor : DuIntervalWithRollbackActionEditor
    {
        private DuProperty m_ScaleBy;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static DuScaleByActionEditor()
        {
            DuActionsPopupButtons.AddActionAnimate(typeof(DuScaleByAction), "ScaleBy");
        }

        [MenuItem("Dust/Actions/ScaleBy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("ScaleBy Action", typeof(DuScaleByAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ScaleBy = FindProperty("m_ScaleBy", "Scale By");
            m_Space = FindProperty("m_Space", "Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuScaleByAction.Parameters"))
            {
                PropertyField(m_ScaleBy);
                OnInspectorGUI_Duration();
                PropertyField(m_Space);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuScaleByAction");
            OnInspectorGUI_Extended("DuScaleByAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
