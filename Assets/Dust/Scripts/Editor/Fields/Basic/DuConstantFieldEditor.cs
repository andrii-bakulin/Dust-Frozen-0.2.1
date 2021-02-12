using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuConstantField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuConstantFieldEditor : DuFieldEditor
    {
        private DuProperty m_Power;
        private DuProperty m_Color;

        //--------------------------------------------------------------------------------------------------------------

        static DuConstantFieldEditor()
        {
            DuFieldsPopupButtons.AddBasicField(typeof(DuConstantField), "Constant");
        }

        [MenuItem("Dust/Fields/Basic Fields/Constant")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuConstantField));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Power = FindProperty("m_Power", "Power");
            m_Color = FindProperty("m_Color", "Color");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();

                PropertyExtendedSlider(m_Power, 0f, 1f, 0.01f);
                PropertyField(m_Color);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
