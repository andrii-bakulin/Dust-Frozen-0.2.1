using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCallRandomAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuCallRandomActionEditor : DuInstantActionEditor
    {
        private DuProperty m_Actions;

        private DuProperty m_Seed;

        //--------------------------------------------------------------------------------------------------------------

        static DuCallRandomActionEditor()
        {
            DuActionsPopupButtons.AddActionFlow(typeof(DuCallRandomAction), "Call Random");
        }

        [MenuItem("Dust/Actions/Call Random")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Call Random Action", typeof(DuCallRandomAction));
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

            if (DustGUI.FoldoutBegin("Parameters", "DuCallRandomAction.Parameters"))
            {
                PropertyField(m_Actions);

                Space();

                PropertySeedRandomOrFixed(m_Seed);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuCallRandomAction");
            OnInspectorGUI_Extended("DuCallRandomAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
