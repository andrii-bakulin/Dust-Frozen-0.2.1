using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/RotateTo Action")]
    public class DuRotateToAction : DuIntervalWithRollbackAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_RotateTo = Vector3.zero;
        public Vector3 rotateTo
        {
            get => m_RotateTo;
            set => m_RotateTo = value;
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        
        protected Quaternion m_RotationStart;
        protected Quaternion m_RotationFinal;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        protected override void OnActionStart()
        {
            base.OnActionStart();

            if (space == Space.World)
            {
                m_RotationStart = m_TargetTransform.rotation;
            }
            else if (space == Space.Local)
            {
                m_RotationStart = m_TargetTransform.localRotation;
            }
             
            m_RotationFinal = Quaternion.Euler(rotateTo);
        }

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TargetTransform))
                return;

            var lerpOffset = 1f;
            var rotateEndPoint = playingPhase == PlayingPhase.Main ? m_RotationFinal : m_RotationStart;

            if (playingPhase == PlayingPhase.Main)
            {
                if (duration > 0f && playbackState < 1f)
                    lerpOffset = deltaTime / ((1f - playbackState) * duration);
            }
            else
            {
                if (rollbackDuration > 0f && playbackState < 1f)
                    lerpOffset = deltaTime / ((1f - playbackState) * rollbackDuration);
            }

            if (space == Space.World)
            {
                m_TargetTransform.rotation = Quaternion.Slerp(m_TargetTransform.rotation, rotateEndPoint, lerpOffset);
            }
            else if (space == Space.Local)
            {
                m_TargetTransform.localRotation = Quaternion.Slerp(m_TargetTransform.localRotation, rotateEndPoint, lerpOffset);
            }
        }
    }
}
