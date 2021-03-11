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
            if (Dust.IsNull(activeTargetTransform))
                return;

            if (playingPhase == PlayingPhase.Main)
                activeTargetTransform.localPosition += m_DeltaLocalMove * (playbackState - previousState);
            else
                activeTargetTransform.localPosition -= m_DeltaLocalMove * (playbackState - previousState);
        }
    }
}
