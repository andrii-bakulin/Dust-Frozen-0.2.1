using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/RotateBy Action")]
    public class DuRotateByAction : DuIntervalAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
            Self = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_RotateBy = Vector3.zero;
        public Vector3 rotateBy
        {
            get => m_RotateBy;
            set => m_RotateBy = value;
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TargetTransform))
                return;

            Vector3 deltaRotate = rotateBy * (percentsCompletedNow - percentsCompletedLast);

            if (space == Space.World)
            {
                m_TargetTransform.Rotate(deltaRotate, UnityEngine.Space.World);
            }
            else if (space == Space.Local)
            {
                if (Dust.IsNotNull(m_TargetTransform.parent))
                    m_TargetTransform.Rotate(m_TargetTransform.parent.TransformDirection(deltaRotate), UnityEngine.Space.World);
                else
                    m_TargetTransform.Rotate(deltaRotate, UnityEngine.Space.World);
            }
            else if (space == Space.Self)
            {
                m_TargetTransform.Rotate(m_TargetTransform.TransformDirection(deltaRotate), UnityEngine.Space.World);
            }
        }
    }
}
