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

        [MenuItem("Dust/Actions/Instant Actions/Callback")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Callback Action", typeof(DuCallbackAction));
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

            OnInspectorGUI_AnyActionFields("DuCallbackAction", true);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
