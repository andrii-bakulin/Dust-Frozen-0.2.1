using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Action MoveBy")]
    public class DuActionMoveBy : DuIntervalAction
    {
        public enum DirectionSpace
        {
            Self = 0,
            Parent = 1,
            World = 2,
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
        private DirectionSpace m_DirectionSpace = DirectionSpace.Parent;
        public DirectionSpace directionSpace
        {
            get => m_DirectionSpace;
            set => m_DirectionSpace = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionUpdate(float deltaTime)
        {
            Transform tr = GetTargetTransform();

            if (Dust.IsNull(tr))
                return;

            Vector3 deltaMove = distance * (percentsCompletedNow - percentsCompletedLast);

            switch (directionSpace)
            {
                case DirectionSpace.Self:
                    tr.localPosition += DuMath.RotatePoint(deltaMove, tr.localEulerAngles);
                    break;

                case DirectionSpace.Parent:
                    tr.localPosition += deltaMove;
                    break;

                case DirectionSpace.World:
                    tr.position += deltaMove;
                    break;
            }
        }
    }
}
