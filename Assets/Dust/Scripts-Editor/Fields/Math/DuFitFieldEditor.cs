using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFitField)), CanEditMultipleObjects]
    public class DuFitFieldEditor : DuFieldEditor
    {
        protected DuProperty m_MinInput;
        protected DuProperty m_MaxInput;

        protected DuProperty m_MinOutput;
        protected DuProperty m_MaxOutput;

        void OnEnable()
        {
            OnEnableField();

            m_MinInput = FindProperty("m_MinInput", "Min Input");
            m_MaxInput = FindProperty("m_MaxInput", "Max Input");

            m_MinOutput = FindProperty("m_MinOutput", "Min Output");
            m_MaxOutput = FindProperty("m_MaxOutput", "Max Output");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyExtendedSlider(m_MinInput, -1f, +2f, 0.01f);
            PropertyExtendedSlider(m_MaxInput, -1f, +2f, 0.01f);

            Space();

            PropertyExtendedSlider(m_MinOutput, -1f, +2f, 0.01f);
            PropertyExtendedSlider(m_MaxOutput, -1f, +2f, 0.01f);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
