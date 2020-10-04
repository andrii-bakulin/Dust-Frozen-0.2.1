﻿using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTorusField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTorusFieldEditor : DuObjectFieldEditor
    {
        private DuProperty m_Radius;
        private DuProperty m_Thickness;
        private DuProperty m_Direction;

        static DuTorusFieldEditor()
        {
            DuPopupButtons.AddObjectField(typeof(DuTorusField), "Torus");
        }

        void OnEnable()
        {
            OnEnableField();

            m_Radius = FindProperty("m_Radius", "Radius");
            m_Thickness = FindProperty("m_Thickness", "Thickness");
            m_Direction = FindProperty("m_Direction", "Direction");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuTorusField.Params"))
            {
                PropertyExtendedSlider(m_Radius, 0f, 10f, 0.01f);
                PropertyExtendedSlider(m_Thickness, 0f, 10f, 0.01f);
                PropertyField(m_Direction);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Radius.isChanged)
                m_Radius.valFloat = DuTorusField.Normalizer.Radius(m_Radius.valFloat);

            if (m_Thickness.isChanged)
                m_Thickness.valFloat = DuTorusField.Normalizer.Thickness(m_Thickness.valFloat);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
