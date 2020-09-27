using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuFactoryMachineEditor : DuEditor
    {
        protected DuProperty m_Strength;

        protected DuFieldsMapEditor m_FieldsMapEditor;

        protected virtual void OnEnableFactoryMachine()
        {
            m_Strength = FindProperty("m_Strength", "Strength");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as DuFactoryMachine).fieldsMap);
        }

        public override void OnInspectorGUI()
        {
            // @todo! drop this?!
            (target as DuMonoBehaviour).enabled = true; // Forced activate all effector-scripts
        }

        protected void OnInspectorGUI_Falloff()
        {
            m_FieldsMapEditor.OnInspectorGUI();
        }
    }
}
