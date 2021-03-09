using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCallbackAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuCallbackActionEditor : DuInstantActionEditor
    {
        static DuCallbackActionEditor()
        {
            DuActionsPopupButtons.AddActionOthers(typeof(DuCallbackAction), "Callback");
        }

        [MenuItem("Dust/Actions/Callback")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Callback Action", typeof(DuCallbackAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            OnInspectorGUI_Callbacks("DuCallbackAction", callbackExpanded:true);
            OnInspectorGUI_Extended("DuCallbackAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
