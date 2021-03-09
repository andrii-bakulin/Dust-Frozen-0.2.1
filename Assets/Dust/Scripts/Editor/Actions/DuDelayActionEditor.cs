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
        private DuProperty m_DurationRangeMin;
        private DuProperty m_DurationRangeMax;
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
            m_DurationRangeMin = FindProperty(serializedObject.FindProperty("m_DurationRange"), "m_Min", "Delay Min Range");
            m_DurationRangeMax = FindProperty(serializedObject.FindProperty("m_DurationRange"), "m_Max", "Delay Max Range");
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
                    OnInspectorGUI_Durations();
                }
                else if (delayMode == DuDelayAction.DelayMode.Range)
                {
                    PropertyDurationSlider(m_DurationRangeMin);
                    PropertyDurationSlider(m_DurationRangeMax);
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
