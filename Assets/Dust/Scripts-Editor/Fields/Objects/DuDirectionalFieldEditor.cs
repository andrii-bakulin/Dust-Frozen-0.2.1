using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDirectionalField)), CanEditMultipleObjects]
    public class DuDirectionalFieldEditor : DuObjectFieldEditor
    {
        private DuProperty m_Length;
        private DuProperty m_Direction;

        private DuProperty m_GizmoWidth;
        private DuProperty m_GizmoHeight;

        void OnEnable()
        {
            OnEnableField();

            m_Length = FindProperty("m_Length", "Length");
            m_Direction = FindProperty("m_Direction", "Direction");

            m_GizmoWidth = FindProperty("m_GizmoWidth", "Width");
            m_GizmoHeight = FindProperty("m_GizmoHeight", "Height");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field", "DuDirectionalField.Params"))
            {
                PropertyExtendedSlider(m_Length, 0f, 10f, 0.01f);
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
                PropertyField(m_GizmoFieldColor);
                PropertyExtendedSlider(m_GizmoWidth, 0f, 10f, 0.1f, 0f);
                PropertyExtendedSlider(m_GizmoHeight, 0f, 10f, 0.1f, 0f);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Length.isChanged)
                m_Length.valFloat = DuDirectionalField.ShapeNormalizer.Length(m_Length.valFloat);

            if (m_GizmoWidth.isChanged)
                m_GizmoWidth.valFloat = DuDirectionalField.ShapeNormalizer.GizmoWidth(m_GizmoWidth.valFloat);

            if (m_GizmoHeight.isChanged)
                m_GizmoHeight.valFloat = DuDirectionalField.ShapeNormalizer.GizmoHeight(m_GizmoHeight.valFloat);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
