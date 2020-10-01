using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuFactoryExtendedMachineEditor : DuFactoryMachineEditor
    {
        protected class DuPropertyGroupPosition
        {
            public SerializedProperty propPositionEnabled;
            public SerializedProperty propPosition;
            public bool isChanged;
        }

        protected class DuPropertyGroupRotation
        {
            public SerializedProperty propRotationEnabled;
            public SerializedProperty propRotation;
            public bool isChanged;
        }

        protected class DuPropertyGroupScale
        {
            public SerializedProperty propScaleEnabled;
            public SerializedProperty propScale;
            public bool isChanged;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected DuProperty m_Min;
        protected DuProperty m_Max;

        protected DuProperty m_TransformMode;
        protected DuProperty m_TransformSpace;

        protected DuPropertyGroupPosition m_GroupPosition;
        protected DuPropertyGroupRotation m_GroupRotation;
        protected DuPropertyGroupScale m_GroupScale;

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

        protected DuProperty m_DebugColorView;

        protected DuFieldsMapEditor m_FieldsMapEditor;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnableFactoryMachine()
        {
            base.OnEnableFactoryMachine();

            m_Min = FindProperty("m_Min", "Min");
            m_Max = FindProperty("m_Max", "Max");

            m_TransformMode = FindProperty("m_TransformMode", "Transform Mode");
            m_TransformSpace = FindProperty("m_TransformSpace", "Transform Space");

            m_GroupPosition = FindPropertyGroupPosition();
            m_GroupRotation = FindPropertyGroupRotation();
            m_GroupScale = FindPropertyGroupScale();

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

            m_DebugColorView = FindProperty("m_DebugColorView", "Color View");

            m_FieldsMapEditor = new DuFieldsMapEditor(this, serializedObject.FindProperty("m_FieldsMap"), (target as DuFactoryExtendedMachine).fieldsMap);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected DuPropertyGroupPosition FindPropertyGroupPosition()
        {
            var duProperty = new DuPropertyGroupPosition
            {
                propPositionEnabled = serializedObject.FindProperty("m_PositionEnabled"),
                propPosition = serializedObject.FindProperty("m_Position"),
                isChanged = false
            };
            return duProperty;
        }

        protected DuPropertyGroupRotation FindPropertyGroupRotation()
        {
            var duProperty = new DuPropertyGroupRotation
            {
                propRotationEnabled = serializedObject.FindProperty("m_RotationEnabled"),
                propRotation = serializedObject.FindProperty("m_Rotation"),
                isChanged = false
            };
            return duProperty;
        }

        protected DuPropertyGroupScale FindPropertyGroupScale()
        {
            var duProperty = new DuPropertyGroupScale
            {
                propScaleEnabled = serializedObject.FindProperty("m_ScaleEnabled"),
                propScale = serializedObject.FindProperty("m_Scale"),
                isChanged = false
            };
            return duProperty;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void OnInspectorGUI_ParametersBlock_IntensityMinMax()
        {
            if (DustGUI.FoldoutBegin("Parameters", "DuFactoryMachineEditor.Parameters"))
            {
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();

                PropertyExtendedSlider(m_Max, -1f, +1f, 0.01f);
                PropertyExtendedSlider(m_Min, -1f, +1f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        protected void OnInspectorGUI_MinMaxBlock()
        {
            if (DustGUI.FoldoutBegin("Min/Max", "DuFactoryMachineEditor.MinMax"))
            {
                PropertyExtendedSlider(m_Max, -1f, +1f, 0.01f);
                PropertyExtendedSlider(m_Min, -1f, +1f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        protected void OnInspectorGUI_TransformBlock()
        {
            if (DustGUI.FoldoutBegin("Transform", "DuFactoryMachineEditor.Transform"))
            {
                PropertyField(m_TransformMode);
                PropertyField(m_TransformSpace);
                Space();
                PropertyBlock(m_GroupPosition);
                Space();
                PropertyBlock(m_GroupRotation);
                Space();
                PropertyBlock(m_GroupScale);
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        protected void OnInspectorGUI_ImpactOnValueBlock()
        {
            if (DustGUI.FoldoutBegin("Impact On Instances Value", "DuFactoryMachineEditor.ImpactOnValue"))
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

        protected void OnInspectorGUI_ColorBlock()
        {
            if (DustGUI.FoldoutBegin("Impact On Instances Color", "DuFactoryMachineEditor.ImpactOnColor"))
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

        protected void OnInspectorGUI_Debug()
        {
            if (DustGUI.FoldoutBegin("Debug", "DuFactoryMachineEditor.Debug", false))
            {
                PropertyField(m_DebugColorView);
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected bool PropertyBlock(DuPropertyGroupPosition duProperty)
        {
            duProperty.isChanged |= PropertyField(duProperty.propPositionEnabled, "Position");
            duProperty.isChanged |= PropertyFieldOrLock(duProperty.propPosition, !duProperty.propPositionEnabled.boolValue, "Offset");
            return duProperty.isChanged;
        }

        protected bool PropertyBlock(DuPropertyGroupRotation duProperty)
        {
            duProperty.isChanged |= PropertyField(duProperty.propRotationEnabled, "Rotation");
            duProperty.isChanged |= PropertyFieldOrLock(duProperty.propRotation, !duProperty.propRotationEnabled.boolValue, "Angle");
            return duProperty.isChanged;
        }

        protected bool PropertyBlock(DuPropertyGroupScale duProperty)
        {
            duProperty.isChanged |= PropertyField(duProperty.propScaleEnabled, "Scale");
            duProperty.isChanged |= PropertyField(duProperty.propScale, "Value");
            return duProperty.isChanged;
        }
    }
}
