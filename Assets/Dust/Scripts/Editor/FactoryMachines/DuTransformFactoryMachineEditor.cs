using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTransformFactoryMachine))]
    [CanEditMultipleObjects]
    public class DuTransformFactoryMachineEditor : DuFactoryExtendedMachineEditor
    {
        protected DuProperty m_Min;
        protected DuProperty m_Max;

        protected DuProperty m_TransformMode;
        protected DuProperty m_TransformSpace;

        protected DuProperty m_PositionEnabled;
        protected DuProperty m_Position;

        protected DuProperty m_RotationEnabled;
        protected DuProperty m_Rotation;

        protected DuProperty m_ScaleEnabled;
        protected DuProperty m_Scale;

        void OnEnable()
        {
            OnEnableFactoryMachine();

            m_Min = FindProperty("m_Min", "Min");
            m_Max = FindProperty("m_Max", "Max");

            m_TransformMode = FindProperty("m_TransformMode", "Transform Mode");
            m_TransformSpace = FindProperty("m_TransformSpace", "Transform Space");

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Position");
            m_Position = FindProperty("m_Position", "Offset");

            m_RotationEnabled = FindProperty("m_RotationEnabled", "Rotation");
            m_Rotation = FindProperty("m_Rotation", "Angle");

            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Scale");
            m_Scale = FindProperty("m_Scale", "Value");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseParameters();

            OnInspectorGUI_TransformBlock();

            OnInspectorGUI_ImpactOnValueBlock();
            OnInspectorGUI_ImpactOnColorBlock();
            OnInspectorGUI_FieldsMap();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }

        protected override void OnInspectorGUI_BaseParameters()
        {
            if (DustGUI.FoldoutBegin("Parameters", "DuFactoryMachine.Parameters"))
            {
                PropertyExtendedSlider(m_Intensity, 0f, 1f, 0.01f);
                Space();

                PropertyExtendedSlider(m_Max, -1f, +1f, 0.01f);
                PropertyExtendedSlider(m_Min, -1f, +1f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();
        }

        protected void OnInspectorGUI_TransformBlock()
        {
            if (DustGUI.FoldoutBegin("Transform", "DuFactoryMachine.Transform"))
            {
                PropertyField(m_TransformMode);
                PropertyField(m_TransformSpace);
                Space();

                PropertyField(m_PositionEnabled);
                PropertyFieldOrLock(m_Position, !m_PositionEnabled.IsTrue);
                Space();

                PropertyField(m_RotationEnabled);
                PropertyFieldOrLock(m_Rotation, !m_RotationEnabled.IsTrue);
                Space();

                PropertyField(m_ScaleEnabled);
                PropertyFieldOrLock(m_Scale, !m_ScaleEnabled.IsTrue);
                Space();
            }
            DustGUI.FoldoutEnd();
        }
    }
}
