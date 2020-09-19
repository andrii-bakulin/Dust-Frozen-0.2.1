using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFieldsSpaceGizmo)), CanEditMultipleObjects]
    public class DuFieldsSpaceGizmoEditor : DuGizmoEditor
    {
        private DuProperty m_FieldsSpace;

        private DuProperty m_GridCount;
        private DuProperty m_GridStep;

        private DuProperty m_PowerVisible;
        private DuProperty m_PowerSize;

        private DuProperty m_ColorVisible;
        private DuProperty m_ColorSize;
        private DuProperty m_ColorAllowTransparent;

        void OnEnable()
        {
            OnEnableGizmo();

            m_FieldsSpace = FindProperty("m_FieldsSpace", "Fields Space");

            m_GridCount = FindProperty("m_GridCount", "Grid Count");
            m_GridStep = FindProperty("m_GridStep", "Grid Step");

            m_PowerVisible = FindProperty("m_PowerVisible", "Visible");
            m_PowerSize = FindProperty("m_PowerSize", "Size");

            m_ColorVisible = FindProperty("m_ColorVisible", "Visible");
            m_ColorSize = FindProperty("m_ColorSize", "Size");
            m_ColorAllowTransparent = FindProperty("m_ColorAllowTransparent", "Allow Transparent");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_FieldsSpace);

            Space();

            if (DustGUI.FoldoutBegin("Grid", "DuFieldsSpaceGizmo.Grid"))
            {
                PropertyField(m_GridCount);
                PropertyField(m_GridStep);
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Power", "DuFieldsSpaceGizmo.Power"))
            {
                PropertyField(m_PowerVisible);
                PropertyExtendedSlider(m_PowerSize, 0.1f, 2.0f, +0.1f, 0.1f);
                Space();
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Color", "DuFieldsSpaceGizmo.Color"))
            {
                PropertyField(m_ColorVisible);
                PropertyExtendedSlider(m_ColorSize, 0.1f, 5.0f, +0.1f, 0.1f);
                PropertyField(m_ColorAllowTransparent);
                Space();
            }
            DustGUI.FoldoutEnd();


            PropertyField(m_GizmoVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
