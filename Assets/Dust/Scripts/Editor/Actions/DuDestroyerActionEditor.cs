using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDestroyerAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuDestroyerActionEditor : DuInstantActionEditor
    {
        private DuProperty m_DisableColliders;

        //--------------------------------------------------------------------------------------------------------------

        static DuDestroyerActionEditor()
        {
            DuActionsPopupButtons.AddActionOthers(typeof(DuDestroyerAction), "Destroyer");
        }

        [MenuItem("Dust/Actions/Instant Actions/Destroyer")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Destroyer Action", typeof(DuDestroyerAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_DisableColliders = FindProperty("m_DisableColliders", "Disable Colliders");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuDestroyerAction.Parameters"))
            {
                PropertyField(m_DisableColliders);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuDestroyerAction");
            OnInspectorGUI_Extended("DuDestroyerAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
