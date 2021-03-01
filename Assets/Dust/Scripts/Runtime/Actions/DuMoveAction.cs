using UnityEngine;

namespace DustEngine
{
    public abstract class DuMoveAction : DuIntervalWithRollbackAction
    {
        protected Vector3 m_DeltaLocalMove;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle
        
        internal override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TargetTransform))
                return;

            if (playingPhase == PlayingPhase.Main)
                m_TargetTransform.localPosition += m_DeltaLocalMove * (playbackStateInPhase - previousStateInPhase);
            else
                m_TargetTransform.localPosition -= m_DeltaLocalMove * (playbackStateInPhase - previousStateInPhase);
        }
    }
}
