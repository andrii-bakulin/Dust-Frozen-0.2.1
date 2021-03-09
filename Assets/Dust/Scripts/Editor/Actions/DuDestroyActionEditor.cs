using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDestroyAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuDestroyActionEditor : DuInstantActionEditor
    {
        private DuProperty m_DisableColliders;

        //--------------------------------------------------------------------------------------------------------------

        static DuDestroyActionEditor()
        {
            DuActionsPopupButtons.AddActionOthers(typeof(DuDestroyAction), "Destroy");
        }

        [MenuItem("Dust/Actions/Destroy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Destroy Action", typeof(DuDestroyAction));
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

            if (DustGUI.FoldoutBegin("Parameters", "DuDestroyAction.Parameters"))
            {
                PropertyField(m_DisableColliders);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuDestroyAction");
            OnInspectorGUI_Extended("DuDestroyAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
