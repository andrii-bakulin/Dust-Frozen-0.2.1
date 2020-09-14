using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTorusField)), CanEditMultipleObjects]
    public class DuTorusFieldGUI : DuObjectFieldEditor
    {
        private DuProperty m_Radius;
        private DuProperty m_Thickness;
        private DuProperty m_Direction;

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

            if (DustGUI.FoldoutBegin("Field", "DuTorusField.Params"))
            {
                PropertyExtendedSlider(m_Radius, 0f, 10f, 0.01f);
                PropertyExtendedSlider(m_Thickness, 0f, 10f, 0.01f);
                PropertyField(m_Direction);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmosBlock();

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
#endif
