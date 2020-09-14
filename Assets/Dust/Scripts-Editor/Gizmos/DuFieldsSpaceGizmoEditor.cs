using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFieldsSpaceGizmo)), CanEditMultipleObjects]
    public class DuFieldsSpaceGizmoEditor : DuGizmoEditor
    {
        private DuProperty m_FieldsSpace;

        private DuProperty m_GridCount;
        private DuProperty m_GridStep;

        private DuProperty m_WeightVisible;
        private DuProperty m_WeightSize;

        private DuProperty m_ColorVisible;
        private DuProperty m_ColorSize;
        private DuProperty m_ColorAllowTransparent;

        void OnEnable()
        {
            OnEnableGizmo();

            m_FieldsSpace = FindProperty("m_FieldsSpace", "Fields Space");

            m_GridCount = FindProperty("m_GridCount", "Grid Count");
            m_GridStep = FindProperty("m_GridStep", "Grid Step");

            m_WeightVisible = FindProperty("m_WeightVisible", "Visible");
            m_WeightSize = FindProperty("m_WeightSize", "Size");

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

            if (DustGUI.FoldoutBegin("Grid"))
            {
                PropertyField(m_GridCount);
                PropertyField(m_GridStep);
            }
            DustGUI.FoldoutEnd();

            if (DustGUI.FoldoutBegin("Weight"))
            {
                PropertyField(m_WeightVisible);
                PropertyExtendedSlider(m_WeightSize, 0.1f, 2.0f, +0.1f, 0.1f);
            }
            DustGUI.FoldoutEnd();

            if (DustGUI.FoldoutBegin("Color"))
            {
                PropertyField(m_ColorVisible);
                PropertyExtendedSlider(m_ColorSize, 0.1f, 5.0f, +0.1f, 0.1f);
                PropertyField(m_ColorAllowTransparent);
            }
            DustGUI.FoldoutEnd();

            Space();

            PropertyField(m_GizmosVisibility);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
