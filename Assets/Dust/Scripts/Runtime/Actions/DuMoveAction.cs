using UnityEngine;

namespace DustEngine
{
    public abstract class DuMoveAction : DuIntervalAction
    {
        protected Vector3 m_DeltaLocalMove;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle
        
        internal override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TargetTransform))
                return;

            m_TargetTransform.localPosition += m_DeltaLocalMove * (percentsCompletedNow - percentsCompletedLast);
        }
    }
}
