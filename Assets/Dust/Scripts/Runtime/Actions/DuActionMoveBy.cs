using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Action MoveBy")]
    public class DuActionMoveBy : DuIntervalAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
            Self = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_Distance = Vector3.forward;
        public Vector3 distance
        {
            get => m_Distance;
            set => m_Distance = value;
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

            Vector3 deltaMove = distance * (percentsCompletedNow - percentsCompletedLast);

            switch (space)
            {
                case Space.World:
                    tr.position += deltaMove;
                    break;

                case Space.Local:
                    tr.localPosition += deltaMove;
                    break;

                case Space.Self:
                    tr.localPosition += DuMath.RotatePoint(deltaMove, tr.localEulerAngles);
                    break;
            }
        }
    }
}
