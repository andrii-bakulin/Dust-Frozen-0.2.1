using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDelayAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuDelayActionEditor : DuIntervalActionEditor
    {
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
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuDelayAction.Parameters"))
            {
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuDelayAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
