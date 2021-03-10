using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRotateToAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuRotateToActionEditor : DuIntervalWithRollbackActionEditor
    {
        private DuProperty m_RotateTo;
        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static DuRotateToActionEditor()
        {
            DuActionsPopupButtons.AddActionAnimate(typeof(DuRotateToAction), "RotateTo");
        }

        [MenuItem("Dust/Actions/RotateTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("RotateTo Action", typeof(DuRotateToAction));
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

            if (DustGUI.FoldoutBegin("Parameters", "DuRotateToAction.Parameters"))
            {
                PropertyField(m_RotateTo);
                OnInspectorGUI_Duration();
                PropertyField(m_Space);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuRotateToAction");
            OnInspectorGUI_Extended("DuRotateToAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
