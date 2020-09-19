using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuWaveDeformer)), CanEditMultipleObjects]
    public class DuWaveDeformerEditor : DuDeformerEditor
    {
        private DuProperty m_Amplitude;
        private DuProperty m_Frequency;
        private DuProperty m_LinearFalloff;
        private DuProperty m_Offset;
        private DuProperty m_AnimationSpeed;

        private DuProperty m_GizmosSize;
        private DuProperty m_GizmosQuality;
        private DuProperty m_GizmosAnimated;

        void OnEnable()
        {
            OnEnableDeformer();

            m_Amplitude = FindProperty("m_Amplitude", "Amplitude");
            m_Frequency = FindProperty("m_Frequency", "Frequency");
            m_LinearFalloff = FindProperty("m_LinearFalloff", "Linear Falloff");
            m_Offset = FindProperty("m_Offset", "Offset");
            m_AnimationSpeed = FindProperty("m_AnimationSpeed", "Animation Speed");

            m_GizmosSize = FindProperty("m_GizmosSize", "Size");
            m_GizmosQuality = FindProperty("m_GizmosQuality", "Quality");
            m_GizmosAnimated = FindProperty("m_GizmosAnimated", "Animate in Editor");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuWaveDeformer.Params"))
            {
                PropertyExtendedSlider(m_Amplitude, 0f, 10f, 0.01f);
                PropertyExtendedSlider(m_Frequency, 0f, 10f, 0.01f);
                PropertyExtendedSlider(m_LinearFalloff, 0f, 10f, 0.01f);
                Space();
                PropertyExtendedSlider(m_Offset, 0f, 1f, 0.01f);
                PropertyExtendedSlider(m_AnimationSpeed, -2f, +2f, 0.01f);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // @ignore: OnInspectorGUI_GizmosBlock();

            if (DustGUI.FoldoutBegin("Gizmos", "DuDeformer.Gizmos"))
            {
                PropertyExtendedSlider(m_GizmosSize, 0.1f, 10f, 0.1f);
                PropertyField(m_GizmosQuality);
                PropertyField(m_GizmosVisibility);
                PropertyField(m_GizmosAnimated);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();

            if (m_GizmosAnimated.isChanged)
                DustGUI.ForcedRedrawSceneView();
        }
    }
}
#endif
