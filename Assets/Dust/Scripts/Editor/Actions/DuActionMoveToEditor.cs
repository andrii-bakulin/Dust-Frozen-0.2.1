using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuActionMoveTo))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuActionMoveToEditor : DuIntervalActionEditor
    {
        private DuProperty m_EndPoint;
        private DuProperty m_EndPointSpace;

        //--------------------------------------------------------------------------------------------------------------

        static DuActionMoveToEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuActionMoveTo), "MoveTo");
        }

        [MenuItem("Dust/Actions/MoveTo")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Action MoveTo", typeof(DuActionMoveTo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_EndPoint = FindProperty("m_EndPoint", "End Point");
            m_EndPointSpace = FindProperty("m_EndPointSpace", "End Point Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuActionMoveTo.Parameters"))
            {
                PropertyField(m_EndPoint);
                PropertyExtendedSlider(m_Duration, 0.00f, 10.0f, +0.01f, 0.00f);
                PropertyField(m_EndPointSpace);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuActionMoveTo");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
