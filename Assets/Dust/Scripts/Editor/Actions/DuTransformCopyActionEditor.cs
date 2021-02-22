﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTransformCopyAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTransformCopyActionEditor : DuInstantActionEditor
    {
        private DuProperty m_SourceObject;
        private DuProperty m_Space;

        private DuProperty m_Position;
        private DuProperty m_Rotation;
        private DuProperty m_Scale;

        private DuTransformCopyAction.Space space => (DuTransformCopyAction.Space) m_Space.enumValueIndex;

        //--------------------------------------------------------------------------------------------------------------

        static DuTransformCopyActionEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuTransformCopyAction), "Transform Copy");
        }

        [MenuItem("Dust/Actions/Instant Actions/Transform Copy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Transform Copy Action", typeof(DuTransformCopyAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_SourceObject = FindProperty("m_SourceObject", "Source Object");
            m_Space = FindProperty("m_Space", "Copy In Space");

            m_Position = FindProperty("m_Position", "Copy Position");
            m_Rotation = FindProperty("m_Rotation", "Copy Rotation");
            m_Scale = FindProperty("m_Scale", "Copy Scale");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuTransformCopyAction.Parameters"))
            {
                PropertyField(m_SourceObject);
                PropertyField(m_Space);

                Space();

                PropertyField(m_Position);
                PropertyField(m_Rotation);
                PropertyField(m_Scale);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_AnyActionFields("DuTransformCopyAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            InspectorCommitUpdates();
        }
    }
}