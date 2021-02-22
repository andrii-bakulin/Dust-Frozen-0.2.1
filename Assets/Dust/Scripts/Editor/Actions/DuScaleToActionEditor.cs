using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuScaleToAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuScaleToActionEditor : DuIntervalActionEditor
    {
        private DuProperty m_ScaleTo;

        //--------------------------------------------------------------------------------------------------------------

        static DuScaleToActionEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuScaleToAction), "ScaleTo");
        }

        [MenuItem("Dust/Actions/ScaleTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("ScaleTo Action", typeof(DuScaleToAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ScaleTo = FindProperty("m_ScaleTo", "Scale To");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuScaleToAction.Parameters"))
            {
                PropertyField(m_ScaleTo);
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuScaleToAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
