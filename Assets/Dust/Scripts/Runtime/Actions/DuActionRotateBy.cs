using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Action RotateBy")]
    public class DuActionRotateBy : DuIntervalAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
            Self = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_Angle = Vector3.forward;
        public Vector3 angle
        {
            get => m_Angle;
            set => m_Angle = value;
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
            Transform tr = GetTargetTransform();

            if (Dust.IsNull(tr))
                return;

            Vector3 deltaRotate = angle * (percentsCompletedNow - percentsCompletedLast);

            switch (space)
            {
                case Space.World:
                    transform.Rotate(deltaRotate, UnityEngine.Space.World);
                    break;

                case Space.Local:
                    transform.Rotate(transform.parent.TransformDirection(deltaRotate), UnityEngine.Space.World);
                    break;

                case Space.Self:
                    transform.Rotate(transform.TransformDirection(deltaRotate), UnityEngine.Space.World);
                    break;
            }
        }
    }
}
