using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDelayAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuDelayActionEditor : DuIntervalActionEditor
    {
        private DuProperty m_DelayMode;
        private DuProperty m_DurationRange;
        private DuProperty m_Seed;

        private DuDelayAction.DelayMode delayMode => (DuDelayAction.DelayMode) m_DelayMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static DuDelayActionEditor()
        {
            DuActionsPopupButtons.AddActionOthers(typeof(DuDelayAction), "Delay");
        }

        [MenuItem("Dust/Actions/Delay")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Delay Action", typeof(DuDelayAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_DelayMode = FindProperty("m_DelayMode", "Mode");
            m_DurationRange = FindProperty("m_DurationRange");
            m_Seed = FindProperty("m_Seed", "Seed");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuDelayAction.Parameters"))
            {
                PropertyField(m_DelayMode);

                if (delayMode == DuDelayAction.DelayMode.Fixed)
                {
                    OnInspectorGUI_Duration();
                }
                else if (delayMode == DuDelayAction.DelayMode.Range)
                {
                    PropertyFieldDurationRange(m_DurationRange); 
                    PropertySeedRandomOrFixed(m_Seed);
                }
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuDelayAction");
            OnInspectorGUI_Extended("DuDelayAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
