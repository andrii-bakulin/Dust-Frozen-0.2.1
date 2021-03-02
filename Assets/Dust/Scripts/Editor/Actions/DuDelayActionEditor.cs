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

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuDelayAction.Parameters"))
            {
                OnInspectorGUI_Durations();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuDelayAction");
            OnInspectorGUI_Extended("DuDelayAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
