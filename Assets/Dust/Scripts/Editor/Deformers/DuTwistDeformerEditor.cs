using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTwistDeformer)), CanEditMultipleObjects]
    public class DuTwistDeformerEditor : DuDeformerEditor
    {
        private DuProperty m_ImpactMode;
        private DuProperty m_Size;
        private DuProperty m_Angle;

        void OnEnable()
        {
            OnEnableDeformer();

            m_ImpactMode = FindProperty("m_ImpactMode", "Impact Mode");
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
                PropertyField(m_ImpactMode);
                PropertyField(m_Size);
                PropertyExtendedSlider(m_Angle, -360f, 360f, 1f);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_FieldsMap();
            OnInspectorGUI_GizmosBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Size.isChanged)
                m_Size.valVector3 = DuTwistDeformer.Normalizer.Size(m_Size.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
