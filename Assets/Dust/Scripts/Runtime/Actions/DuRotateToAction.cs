using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/RotateTo Action")]
    public class DuRotateToAction : DuIntervalAction
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

        protected Quaternion m_RotationFinal;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionStart()
        {
            base.OnActionStart();
            
            m_RotationFinal = Quaternion.Euler(rotateTo);
        }

        internal override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TargetTransform))
                return;

            var lerpOffset = duration > 0f && percentsCompletedNow < 1f
                ? deltaTime / ((1f - percentsCompletedNow) * duration)
                : 1f;

            if (space == Space.World)
            {
                m_TargetTransform.rotation = Quaternion.Slerp(m_TargetTransform.rotation, m_RotationFinal, lerpOffset);
            }
            else if (space == Space.Local)
            {
                m_TargetTransform.localRotation = Quaternion.Slerp(m_TargetTransform.localRotation, m_RotationFinal, lerpOffset);
            }
        }
    }
}
