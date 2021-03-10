using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFlowRandomAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuFlowRandomActionEditor : DuFlowActionEditor
    {
        private DuProperty m_Actions;

        private DuProperty m_Seed;

        //--------------------------------------------------------------------------------------------------------------

        static DuFlowRandomActionEditor()
        {
            DuActionsPopupButtons.AddActionFlow(typeof(DuFlowRandomAction), "Flow Random");
        }

        [MenuItem("Dust/Actions/Flow Random")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Flow Random Action", typeof(DuFlowRandomAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Actions = FindProperty("m_Actions", "Actions");

            m_Seed = FindProperty("m_Seed");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuFlowRandomAction.Parameters"))
            {
                PropertyField(m_Actions);

                Space();

                PropertySeedRandomOrFixed(m_Seed);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Extended("DuFlowRandomAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
