using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTwistDeformer)), CanEditMultipleObjects]
    public class DuTwistDeformerEditor : DuDeformerEditor
    {
        private DuProperty m_DeformMode;
        private DuProperty m_Size;
        private DuProperty m_Angle;

        void OnEnable()
        {
            OnEnableDeformer();

            m_DeformMode = FindProperty("m_DeformMode", "Deform Mode");
            m_Size = FindProperty("m_Size", "Size");
            m_Angle = FindProperty("m_Angle", "Angle");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuTwistDeformer.Params"))
            {
                PropertyField(m_Size);
                PropertyExtendedSlider(m_Angle, -360f, 360f, 1f);
                PropertyField(m_DeformMode);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_FieldsMap();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Size.isChanged)
                m_Size.valVector3 = DuTwistDeformer.Normalizer.Size(m_Size.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Require forced redraw scene view

            DustGUI.ForcedRedrawSceneView();
        }
    }
}
