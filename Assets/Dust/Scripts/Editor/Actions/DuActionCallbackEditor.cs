using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuActionCallback))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuActionCallbackEditor : DuInstantActionEditor
    {
        private DuProperty m_Callback;

        //--------------------------------------------------------------------------------------------------------------

        static DuActionCallbackEditor()
        {
            DuActionsPopupButtons.AddActionOthers(typeof(DuActionCallback), "Callback");
        }

        [MenuItem("Dust/Actions/Callback")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Action Callback", typeof(DuActionCallback));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Callback = FindProperty("m_Callback", "Callback");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            PropertyField(m_Callback);

            // @todo: It's strange to show "Extended" block here!
            OnInspectorGUI_AnyActionFields("DuActionCallback");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
