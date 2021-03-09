using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFlipAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuFlipActionEditor : DuInstantActionEditor
    {
        private DuProperty m_FlipX;
        private DuProperty m_FlipY;
        private DuProperty m_FlipZ;

        //--------------------------------------------------------------------------------------------------------------

        static DuFlipActionEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuFlipAction), "Flip");
        }

        [MenuItem("Dust/Actions/Flip")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Flip Action", typeof(DuFlipAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_FlipX = FindProperty("m_FlipX", "FlipX");
            m_FlipY = FindProperty("m_FlipY", "FlipY");
            m_FlipZ = FindProperty("m_FlipZ", "FlipZ");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuFlipAction.Parameters"))
            {
                PropertyField(m_FlipX);
                PropertyField(m_FlipY);
                PropertyField(m_FlipZ);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuFlipAction");
            OnInspectorGUI_Extended("DuFlipAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
