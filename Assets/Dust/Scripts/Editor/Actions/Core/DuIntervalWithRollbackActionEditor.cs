using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuIntervalWithRollbackActionEditor : DuActionEditor
    {
        protected DuProperty m_Duration;
        protected DuProperty m_PlayRollback;
        protected DuProperty m_RollbackDuration;

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_Duration = FindProperty("m_Duration", "Duration");
            m_PlayRollback = FindProperty("m_PlayRollback", "Play Rollback");
            m_RollbackDuration = FindProperty("m_RollbackDuration", "Rollback Duration");
        }

        protected void CheckDurationsStates()
        {
            if (!m_PlayRollback.IsTrue)
                return;

            if (DuMath.IsNotZero(m_Duration.valFloat) || DuMath.IsNotZero(m_RollbackDuration.valFloat))
                return;
            
            DustGUI.HelpBoxWarning("This action has rollback flag and both durations have zero-lengths, so action has no sense.");
        }

        protected override void InspectorCommitUpdates()
        {
            if (m_Duration.isChanged)
                m_Duration.valFloat = DuIntervalWithRollbackAction.Normalizer.Duration(m_Duration.valFloat);

            if (m_RollbackDuration.isChanged)
                m_RollbackDuration.valFloat = DuIntervalWithRollbackAction.Normalizer.Duration(m_RollbackDuration.valFloat);

            base.InspectorCommitUpdates();
        }
    }
}
