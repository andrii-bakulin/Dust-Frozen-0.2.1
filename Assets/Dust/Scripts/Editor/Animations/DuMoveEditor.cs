using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuMove))]
    [CanEditMultipleObjects]
    public class DuMoveEditor : DuEditor
    {
        private DuProperty m_TranslateType;

        private DuProperty m_LinearSpeed;

        private DuProperty m_WaveAmplitude;
        private DuProperty m_WaveSpeed;
        private DuProperty m_WaveOffset;

        private DuProperty m_DirectionSpace;
        private DuProperty m_Freeze;

        private DuProperty m_UpdateMode;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animations/Move")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Move", typeof(DuMove));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_TranslateType = FindProperty("m_TranslateType", "Translate Kind");

            m_LinearSpeed = FindProperty("m_LinearSpeed", "Speed");

            m_WaveAmplitude = FindProperty("m_WaveAmplitude", "Amplitude");
            m_WaveSpeed = FindProperty("m_WaveSpeed", "Speed in Degrees");
            m_WaveOffset = FindProperty("m_WaveOffset", "Offset");

            m_DirectionSpace = FindProperty("m_DirectionSpace", "Direction Space");
            m_Freeze = FindProperty("m_Freeze", "Freeze");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_TranslateType);

            switch ((DuMove.TranslateType) m_TranslateType.enumValueIndex)
            {
                case DuMove.TranslateType.Linear:
                    PropertyField(m_LinearSpeed);
                    break;

                case DuMove.TranslateType.Wave:
                    PropertyField(m_WaveAmplitude);
                    PropertyField(m_WaveSpeed);
                    PropertyField(m_WaveOffset);
                    break;
            }

            Space();

            PropertyField(m_DirectionSpace);
            PropertyField(m_Freeze);

            Space();

            PropertyField(m_UpdateMode);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Validate & Normalize Data

            if (GUI.changed)
                m_WaveOffset.valVector3 = DuVector3.Clamp01(m_WaveOffset.valVector3);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
