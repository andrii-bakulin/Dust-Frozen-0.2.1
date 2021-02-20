﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuParallaxInstance))]
    [CanEditMultipleObjects]
    public class DuParallaxInstanceEditor : DuEditor
    {
        private DuProperty m_ParentParallax;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_ParentParallax = FindProperty("m_ParentParallax", "Parent Parallax");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyFieldOrLock(m_ParentParallax, true);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
