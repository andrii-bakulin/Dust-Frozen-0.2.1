using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuFieldsSpace))]
    public class DuFieldsSpaceEditor : DuEditor
    {
        private DuProperty m_CalculatePower;
        private DuProperty m_DefaultPower;

        private DuProperty m_CalculateColor;
        private DuProperty m_DefaultColor;

        private DuFieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        protected void OnEnable()
        {
            SerializedProperty propertyFieldsMap = serializedObject.FindProperty("m_FieldsMap");

            m_CalculatePower = FindProperty(propertyFieldsMap, "m_CalculatePower", "Calculate");
            m_DefaultPower = FindProperty(propertyFieldsMap, "m_DefaultPower", "Default");

            m_CalculateColor = FindProperty(propertyFieldsMap, "m_CalculateColor", "Calculate");
            m_DefaultColor = FindProperty(propertyFieldsMap, "m_DefaultColor", "Default");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, propertyFieldsMap, (target as DuFieldsSpace).fieldsMap);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Power", "DuFieldsSpace.Power"))
            {
                PropertyField(m_CalculatePower);

                if (!m_CalculatePower.IsTrue)
                    DustGUI.Lock();

                PropertyField(m_DefaultPower);

                DustGUI.Unlock();
                Space();
            }
            DustGUI.FoldoutEnd();

            if (DustGUI.FoldoutBegin("Color", "DuFieldsSpace.Color"))
            {
                PropertyField(m_CalculateColor);

                if (!m_CalculateColor.IsTrue)
                    DustGUI.Lock();

                PropertyField(m_DefaultColor);

                DustGUI.Unlock();
                Space();
            }
            DustGUI.FoldoutEnd();

            m_FieldsMapEditor.OnInspectorGUI();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
