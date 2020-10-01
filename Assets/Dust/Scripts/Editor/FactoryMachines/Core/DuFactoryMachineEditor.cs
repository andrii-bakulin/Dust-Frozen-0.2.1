using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuFactoryMachineEditor : DuEditor
    {
        protected DuProperty m_Intensity;

        protected virtual void OnEnableFactoryMachine()
        {
            m_Intensity = FindProperty("m_Intensity", "Intensity");
        }

        public override void OnInspectorGUI()
        {
            // Hide default OnInspectorGUI() call
            // Extend all-factoryMachines-view if need in future...
        }

        protected virtual void OnInspectorGUI_BaseParameters()
        {
            if (DustGUI.FoldoutBegin("Parameters", "DuFactoryMachine.Parameters"))
            {
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
