using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuFactoryExtendedMachineEditor : DuFactoryMachineEditor
    {
        protected DuProperty m_ValueImpactSource;
        protected DuProperty m_ValueImpactIntensity;
        protected DuProperty m_ValueBlendMode;
        protected DuProperty m_ValueFixed;
        protected DuProperty m_ValueClampEnabled;
        protected DuProperty m_ValueClampMin;
        protected DuProperty m_ValueClampMax;

        protected DuProperty m_ColorImpactSource;
        protected DuProperty m_ColorImpactIntensity;
        protected DuProperty m_ColorBlendMode;
        protected DuProperty m_ColorFixed;

        protected DuFieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnableFactoryMachine()
        {
            base.OnEnableFactoryMachine();

            m_ValueImpactSource = FindProperty("m_ValueImpactSource", "Source");
            m_ValueImpactIntensity = FindProperty("m_ValueImpactIntensity", "Intensity");
            m_ValueBlendMode = FindProperty("m_ValueBlendMode", "Blend Mode");
            m_ValueFixed = FindProperty("m_ValueFixed", "Fixed Value");
            m_ValueClampEnabled = FindProperty("m_ValueClampEnabled", "Clamp");
            m_ValueClampMin = FindProperty("m_ValueClampMin", "Min");
            m_ValueClampMax = FindProperty("m_ValueClampMax", "Max");

            m_ColorImpactSource = FindProperty("m_ColorImpactSource", "Source");
            m_ColorImpactIntensity = FindProperty("m_ColorImpactIntensity", "Intensity");
            m_ColorBlendMode = FindProperty("m_ColorBlendMode", "Blend Mode");
            m_ColorFixed = FindProperty("m_ColorFixed", "Fixed Color");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as DuFactoryExtendedMachine).fieldsMap);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void OnInspectorGUI_ImpactOnValueBlock()
        {
            if (DustGUI.FoldoutBegin("Impact On Instances Value", "DuFactoryMachine.ImpactOnValue"))
            {
                PropertyField(m_ValueImpactSource);

                var valueImpactSource = (DuFactoryExtendedMachine.ValueImpactSource) m_ValueImpactSource.enumValueIndex;

                if (valueImpactSource != DuFactoryExtendedMachine.ValueImpactSource.Skip)
                {
                    if (valueImpactSource == DuFactoryExtendedMachine.ValueImpactSource.FixedValue)
                        PropertyField(m_ValueFixed);

                    PropertyField(m_ValueBlendMode);
                    PropertyExtendedSlider(m_ValueImpactIntensity, 0f, +1f, 0.01f);

                    Space();

                    PropertyField(m_ValueClampEnabled);

                    if (m_ValueClampEnabled.IsTrue)
                    {
                        DustGUI.IndentLevelInc();
                        PropertyExtendedSlider(m_ValueClampMin, -1f, +1f, 0.01f);
                        PropertyExtendedSlider(m_ValueClampMax, -1f, +1f, 0.01f);
                        DustGUI.IndentLevelDec();
                    }
                }
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        protected void OnInspectorGUI_ImpactOnColorBlock()
        {
            if (DustGUI.FoldoutBegin("Impact On Instances Color", "DuFactoryMachine.ImpactOnColor"))
            {
                PropertyField(m_ColorImpactSource);

                var colorImpactSource = (DuFactoryExtendedMachine.ColorImpactSource) m_ColorImpactSource.enumValueIndex;

                if (colorImpactSource != DuFactoryExtendedMachine.ColorImpactSource.Skip)
                {
                    if (colorImpactSource == DuFactoryExtendedMachine.ColorImpactSource.FixedColor)
                        PropertyField(m_ColorFixed);

                    PropertyField(m_ColorBlendMode);
                    PropertyExtendedSlider(m_ColorImpactIntensity, 0f, +1f, 0.01f);
                }
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        protected void OnInspectorGUI_Falloff()
        {
            m_FieldsMapEditor.OnInspectorGUI();
        }
    }
}
