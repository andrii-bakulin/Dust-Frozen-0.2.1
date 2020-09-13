﻿using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTorusGizmo)), CanEditMultipleObjects]
    public class DuTorusGizmoEditor : DuGizmoEditor
    {
        private DuProperty m_Radius;
        private DuProperty m_Thickness;
        private DuProperty m_Center;
        private DuProperty m_Direction;

        void OnEnable()
        {
            OnEnableGizmo();

            m_Radius = FindProperty("m_Radius", "Radius");
            m_Thickness = FindProperty("m_Thickness", "Thickness");
            m_Center = FindProperty("m_Center", "Center");
            m_Direction = FindProperty("m_Direction", "Direction");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyExtendedSlider(m_Radius, 0f, 10f, 0.01f);
            PropertyExtendedSlider(m_Thickness, 0f, 10f, 0.01f);
            PropertyField(m_Center);
            PropertyField(m_Direction);
            Space();
            PropertyField(m_Color);
            PropertyField(m_GizmosVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif