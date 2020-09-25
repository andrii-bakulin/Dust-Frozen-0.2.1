using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuRadialFactory))]
    [CanEditMultipleObjects]
    public class DuRadialFactoryEditor : DuFactoryEditor
    {
        private DuProperty m_Count;
        private DuProperty m_Radius;
        private DuProperty m_Orientation;
        private DuProperty m_Align;
        private DuProperty m_StartAngle;
        private DuProperty m_EndAngle;
        private DuProperty m_Offset;
        private DuProperty m_OffsetVariation;
        private DuProperty m_OffsetSeed;

        void OnEnable()
        {
            OnEnableFactory();

            m_Count = FindProperty("m_Count", "Count");
            m_Radius = FindProperty("m_Radius", "Radius");
            m_Orientation = FindProperty("m_Orientation", "Orientation");
            m_Align = FindProperty("m_Align", "Align");
            m_StartAngle = FindProperty("m_StartAngle", "Start Angle");
            m_EndAngle = FindProperty("m_EndAngle", "End Angle");
            m_Offset = FindProperty("m_Offset", "Offset");
            m_OffsetVariation = FindProperty("m_OffsetVariation", "Offset Variation");
            m_OffsetSeed = FindProperty("m_OffsetSeed", "Offset Seed");
        }

        public override void OnInspectorGUI()
        {
            BeginData();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Radial", "DuFactory.Radial"))
            {
                PropertyExtendedIntSlider(m_Count, 1, 100, 1, 1);
                PropertyExtendedSlider(m_Radius, 0f, 10f, 0.01f);
                PropertyField(m_Orientation);
                PropertyField(m_Align);
                Space();
                PropertyExtendedSlider(m_StartAngle, 0f, 360f, 1f);
                PropertyExtendedSlider(m_EndAngle, 0f, 360f, 1f);
                Space();
                PropertyExtendedSlider(m_Offset, 0f, 360f, 1f);
                PropertyExtendedSlider(m_OffsetVariation, 0f, 1f, 0.01f);
                PropertySeedFixed(m_OffsetSeed);
                Space();
            }
            DustGUI.FoldoutEnd();

            m_IsRequireRebuildClones |= m_Count.isChanged;

            m_IsRequireResetupClones |= m_Radius.isChanged;
            m_IsRequireResetupClones |= m_Orientation.isChanged;
            m_IsRequireResetupClones |= m_Align.isChanged;
            m_IsRequireResetupClones |= m_StartAngle.isChanged;
            m_IsRequireResetupClones |= m_EndAngle.isChanged;
            m_IsRequireResetupClones |= m_Offset.isChanged;
            m_IsRequireResetupClones |= m_OffsetVariation.isChanged;
            m_IsRequireResetupClones |= m_OffsetSeed.isChanged;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_Objects();
            OnInspectorGUI_FactoryMachines();
            OnInspectorGUI_Transform();
            OnInspectorGUI_Display();
            OnInspectorGUI_Tools();

            //----------------------------------------------------------------------------------------------------------
            // Validate & Normalize Data

            if (m_Count.isChanged)
                m_Count.valInt = DuRadialFactory.Normalizer.Count(m_Count.valInt);

            if (m_OffsetSeed.isChanged)
                m_OffsetSeed.valInt = DuRadialFactory.Normalizer.OffsetSeed(m_OffsetSeed.valInt);

            //----------------------------------------------------------------------------------------------------------

            CommitDataAndUpdateStates();
        }
    }
}
