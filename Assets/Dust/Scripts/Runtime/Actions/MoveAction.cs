using UnityEngine;

namespace DustEngine
{
    public abstract class MoveAction : IntervalWithRollbackAction
    {
        protected Vector3 m_DeltaLocalMove;

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle
        
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
