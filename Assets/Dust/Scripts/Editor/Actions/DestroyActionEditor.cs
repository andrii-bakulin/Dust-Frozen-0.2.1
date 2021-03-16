using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DestroyAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DestroyActionEditor : InstantActionEditor
    {
        private DuProperty m_DisableColliders;

        //--------------------------------------------------------------------------------------------------------------

        static DestroyActionEditor()
        {
            ActionsPopupButtons.AddActionOthers(typeof(DestroyAction), "Destroy");
        }

        [MenuItem("Dust/Actions/Destroy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Destroy Action", typeof(DestroyAction));
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

            if (DustGUI.FoldoutBegin("Parameters", "DestroyAction.Parameters"))
            {
                PropertyField(m_DisableColliders);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DestroyAction");
            OnInspectorGUI_Extended("DestroyAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
