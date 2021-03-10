using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuIntervalActionEditor : DuActionEditor
    {
        protected DuProperty m_Duration;

        protected DuProperty m_RepeatMode;
        protected DuProperty m_RepeatTimes;

        protected DuIntervalAction.RepeatMode repeatMode => (DuIntervalAction.RepeatMode) m_RepeatMode.valInt;

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Duration = FindProperty("m_Duration", "Duration");

            m_RepeatMode = FindProperty("m_RepeatMode", "Repeat Mode");
            m_RepeatTimes = FindProperty("m_RepeatTimes", "Repeat Times");
        }

        protected virtual void OnInspectorGUI_Duration()
        {
            PropertyDurationField(m_Duration);
        }

        protected override void OnInspectorGUI_Extended_BlockMiddle()
        {
            PropertyField(m_RepeatMode);
            
            if (repeatMode == DuIntervalAction.RepeatMode.Repeat)
                PropertyExtendedIntSlider(m_RepeatTimes, 1, 50, 1, 1);
        }

        protected override void InspectorCommitUpdates()
        {
            if (m_Duration.isChanged)
                m_Duration.valFloat = DuIntervalAction.Normalizer.Duration(m_Duration.valFloat);

            if (m_RepeatTimes.isChanged)
                m_RepeatTimes.valInt = DuIntervalAction.Normalizer.RepeatTimes(m_RepeatTimes.valInt);

            base.InspectorCommitUpdates();
        }
    }
}
