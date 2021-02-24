using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTransformSetAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTransformSetActionEditor : DuInstantActionEditor
    {
        private DuProperty m_AssignPosition;
        private DuProperty m_AssignRotation;
        private DuProperty m_AssignScale;

        private DuProperty m_Position;
        private DuProperty m_Rotation;
        private DuProperty m_Scale;

        private DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        static DuTransformSetActionEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuTransformSetAction), "Transform Set");
        }

        [MenuItem("Dust/Actions/Instant Actions/Transform Set")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Transform Set Action", typeof(DuTransformSetAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_AssignPosition = FindProperty("m_AssignPosition", "Assign Position");
            m_AssignRotation = FindProperty("m_AssignRotation", "Assign Rotation");
            m_AssignScale = FindProperty("m_AssignScale", "Assign Scale");

            m_Position = FindProperty("m_Position", "Set Position");
            m_Rotation = FindProperty("m_Rotation", "Set Rotation");
            m_Scale = FindProperty("m_Scale", "Set Scale");

            m_Space = FindProperty("m_Space", "Set In Space");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuTransformSetAction.Parameters"))
            {
                PropertyField(m_AssignPosition);
                PropertyField(m_AssignRotation);
                PropertyField(m_AssignScale);
                PropertyFieldOrHide(m_Position, !m_AssignPosition.IsTrue);
                PropertyFieldOrHide(m_Rotation, !m_AssignRotation.IsTrue);
                PropertyFieldOrHide(m_Scale, !m_AssignScale.IsTrue);

                Space();

                PropertyField(m_Space);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuTransformSetAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}
