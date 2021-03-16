using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuActivateAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuActivateActionEditor : DuInstantActionEditor
    {
        private DuProperty m_Action;
        private DuProperty m_GameObjects;
        private DuProperty m_Components;

        private DuProperty m_ApplyToSelf;
        private DuProperty m_Seed;

        private DuActivateAction.Action action => (DuActivateAction.Action) m_Action.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static DuActivateActionEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuActivateAction), "Activate");
        }

        [MenuItem("Dust/Actions/Activate")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Activate Action", typeof(DuActivateAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Action = FindProperty("m_Action", "Action");
            m_GameObjects = FindProperty("m_GameObjects", "Game Objects");
            m_Components = FindProperty("m_Components", "Components");

            m_ApplyToSelf = FindProperty("m_ApplyToSelf", "Apply To Self");
            m_Seed = FindProperty("m_Seed");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuActivateAction.Parameters"))
            {
                PropertyField(m_Action);

                if (action == DuActivateAction.Action.ToggleRandom)
                    PropertySeedRandomOrFixed(m_Seed);

                Space();
                
                PropertyField(m_GameObjects);
                PropertyField(m_Components);

                Space();
                
                PropertyField(m_ApplyToSelf);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuActivateAction");
            OnInspectorGUI_Extended("DuActivateAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
