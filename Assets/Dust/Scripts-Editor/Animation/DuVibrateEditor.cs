using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuVibrate)), CanEditMultipleObjects]
    public class DuVibrateEditor : DuEditor
    {
        private DuProperty m_Uniform;
        private DuProperty m_Seed;
        private DuProperty m_Force;
        private DuProperty m_Freeze;

        private DuProperty m_PositionEnabled;
        private DuProperty m_PositionAmplitude;
        private DuProperty m_PositionFrequency;

        private DuProperty m_RotationEnabled;
        private DuProperty m_RotationAmplitude;
        private DuProperty m_RotationFrequency;

        private DuProperty m_ScaleEnabled;
        private DuProperty m_ScaleUniform;
        private DuProperty m_ScaleAmplitude;
        private DuProperty m_ScaleFrequency;

        private DuProperty m_TransformMode;

        private DuProperty m_UpdateMode;

        void OnEnable()
        {
            m_Uniform = FindProperty("m_Uniform", "Uniform Vibration");
            m_Seed = FindProperty("m_Seed", "Seed");
            m_Force = FindProperty("m_Force", "Force");
            m_Freeze = FindProperty("m_Freeze", "Freeze");

            m_PositionEnabled = FindProperty("m_PositionEnabled", "Enable");
            m_PositionAmplitude = FindProperty("m_PositionAmplitude", "Amplitude");
            m_PositionFrequency = FindProperty("m_PositionFrequency", "Frequency");

            m_RotationEnabled = FindProperty("m_RotationEnabled", "Enable");
            m_RotationAmplitude = FindProperty("m_RotationAmplitude", "Amplitude");
            m_RotationFrequency = FindProperty("m_RotationFrequency", "Frequency");

            m_ScaleEnabled = FindProperty("m_ScaleEnabled", "Enable");
            m_ScaleUniform = FindProperty("m_ScaleUniform", "Uniform Scale");
            m_ScaleAmplitude = FindProperty("m_ScaleAmplitude", "Amplitude");
            m_ScaleFrequency = FindProperty("m_ScaleFrequency", "Frequency");

            m_TransformMode = FindProperty("m_TransformMode", "Transform Mode");
            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Position", "DuVibrate.Position"))
            {
                PropertyField(m_PositionEnabled);

                if (m_PositionEnabled.IsTrue)
                {
                    PropertyField(m_PositionAmplitude);
                    PropertyExtendedSlider(m_PositionFrequency, 0f, 10f, 0.01f);
                }
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Rotation", "DuVibrate.Rotation"))
            {
                PropertyField(m_RotationEnabled);

                if (m_RotationEnabled.IsTrue)
                {
                    PropertyField(m_RotationAmplitude);
                    PropertyExtendedSlider(m_RotationFrequency, 0f, 10f, 0.01f);
                }
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Scale", "DuVibrate.Scale"))
            {
                PropertyField(m_ScaleEnabled);

                if (m_ScaleEnabled.IsTrue)
                {
                    PropertyFieldOrLock(m_ScaleUniform, m_Uniform.IsTrue);
                    PropertyField(m_ScaleAmplitude);
                    PropertyExtendedSlider(m_ScaleFrequency, 0f, 10f, 0.01f);
                }
            }
            DustGUI.FoldoutEnd();


            if (DustGUI.FoldoutBegin("Parameters", "DuVibrate.Parameters"))
            {
                PropertyField(m_Uniform);

                if (!m_Uniform.IsTrue)
                    PropertySeedRandomOrFixed(m_Seed);

                Space();

                PropertyExtendedSlider(m_Force, 0f, 1f, 0.01f, 0f, 1f);
                PropertyField(m_Freeze);

                Space();

                PropertyField(m_TransformMode);

                if ((DuVibrate.TransformMode) m_TransformMode.enumValueIndex == DuVibrate.TransformMode.AppendToAnimation)
                    DustGUI.HelpBoxInfo("This mode need to use when object animated by keyframes or manually in Update method."
                                        + " Then you may apply vibration in LastUpdate calls");

                Space();

                PropertyField(m_UpdateMode);
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (m_Force.isChanged)
                m_Force.valFloat = DuVibrate.Normalizer.Force(m_Force.valFloat);

            if (m_ScaleAmplitude.isChanged)
                m_ScaleAmplitude.valVector3 = DuVibrate.Normalizer.ScaleAmplitude(m_ScaleAmplitude.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
