using UnityEngine;

namespace DustEngine
{
    public abstract class DuMoveAction : DuIntervalWithRollbackAction
    {
        protected Vector3 m_DeltaLocalMove;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle
        
        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TargetTransform))
                return;

            if (playingPhase == PlayingPhase.Main)
                m_TargetTransform.localPosition += m_DeltaLocalMove * (playbackState - previousState);
            else
                m_TargetTransform.localPosition -= m_DeltaLocalMove * (playbackState - previousState);
        }
    }
}
