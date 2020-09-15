using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRadialField)), CanEditMultipleObjects]
    public class DuRadialFieldEditor : DuObjectFieldEditor
    {
        private DuProperty m_StartAngle;
        private DuProperty m_EndAngle;

        private DuProperty m_FadeInOffset;
        private DuProperty m_FadeOutOffset;

        private DuProperty m_Iterations;
        private DuProperty m_Offset;

        private DuProperty m_Direction;

        private DuProperty m_GizmoLength;
        private DuProperty m_GizmoRadius;

        void OnEnable()
        {
            OnEnableField();

            m_StartAngle = FindProperty("m_StartAngle", "Start Angle");
            m_EndAngle = FindProperty("m_EndAngle", "End Angle");

            m_FadeInOffset = FindProperty("m_FadeInOffset", "Fade In Offset");
            m_FadeOutOffset = FindProperty("m_FadeOutOffset", "Fade Out Offset");

            m_Iterations = FindProperty("m_Iterations", "Iterations");
            m_Offset = FindProperty("m_Offset", "Offset");

            m_Direction = FindProperty("m_Direction", "Direction");

            m_GizmoLength = FindProperty("m_GizmoLength", "Length");
            m_GizmoRadius = FindProperty("m_GizmoRadius", "Radius");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field", "DuRadialField.Params"))
            {
                PropertyExtendedSlider(m_StartAngle, 0f, 360f, 1f);
                PropertyExtendedSlider(m_EndAngle, 0f, 360f, 1f);
                Space();
                PropertyExtendedSlider(m_FadeInOffset, 0f, 360f, 1f);
                PropertyExtendedSlider(m_FadeOutOffset, 0f, 360f, 1f);
                Space();
                PropertyExtendedSlider(m_Iterations, 1f, 10f, 0.01f, 1f);
                PropertyExtendedSlider(m_Offset, 0f, 360f, 1f);
                Space();
                PropertyField(m_Direction);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // OnInspectorGUI_GizmosBlock();

            if (DustGUI.FoldoutBegin("Gizmos", "DuField.Gizmos"))
            {
                PropertyField(m_GizmosVisibility);
                PropertyExtendedSlider(m_GizmoLength, 0f, 10f, 0.1f, 0f);
                PropertyExtendedSlider(m_GizmoRadius, 0f, 5f, 0.1f, 0f);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_FadeInOffset.isChanged)
                m_FadeInOffset.valFloat = DuRadialField.ShapeNormalizer.FadeOffset(m_FadeInOffset.valFloat);

            if (m_FadeOutOffset.isChanged)
                m_FadeOutOffset.valFloat = DuRadialField.ShapeNormalizer.FadeOffset(m_FadeOutOffset.valFloat);

            if (m_Iterations.isChanged)
                m_Iterations.valFloat = DuRadialField.ShapeNormalizer.Iterations(m_Iterations.valFloat);

            if (m_GizmoLength.isChanged)
                m_GizmoLength.valFloat = DuRadialField.ShapeNormalizer.GizmoLength(m_GizmoLength.valFloat);

            if (m_GizmoRadius.isChanged)
                m_GizmoRadius.valFloat = DuRadialField.ShapeNormalizer.GizmoRadius(m_GizmoRadius.valFloat);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
