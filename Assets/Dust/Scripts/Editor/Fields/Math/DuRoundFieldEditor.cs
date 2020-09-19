﻿using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRoundField)), CanEditMultipleObjects]
    public class DuRoundFieldEditor : DuFieldEditor
    {
        protected DuProperty m_RoundMode;
        protected DuProperty m_Distance;

        void OnEnable()
        {
            OnEnableField();

            m_RoundMode = FindProperty("m_RoundMode", "Round Mode");
            m_Distance = FindProperty("m_Distance", "Distance");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_RoundMode);
            PropertyExtendedSlider(m_Distance, 0f, 1f, 0.01f);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif