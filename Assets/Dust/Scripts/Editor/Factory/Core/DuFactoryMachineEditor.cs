using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuFactoryMachineEditor : DuEditor
    {
        protected DuProperty m_Intensity;

        protected DuFieldsMapEditor m_FieldsMapEditor;

        protected virtual void OnEnableFactoryMachine()
        {
            m_Intensity = FindProperty("m_Intensity", "Intensity");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as DuFactoryMachine).fieldsMap);
        }

        public override void OnInspectorGUI()
        {
            // Hide default OnInspectorGUI() call
            // Extend all-factoryMachines-view if need in future...
        }

        protected void OnInspectorGUI_Falloff()
        {
            m_FieldsMapEditor.OnInspectorGUI();
        }
    }
}
