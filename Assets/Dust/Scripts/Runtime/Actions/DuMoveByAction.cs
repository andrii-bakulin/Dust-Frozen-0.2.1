using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/MoveBy Action")]
    public class DuMoveByAction : DuIntervalAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
            Self = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_MoveBy = Vector3.zero;
        public Vector3 moveBy
        {
            get => m_MoveBy;
            set => m_MoveBy = value;
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

            Vector3 deltaMove = moveBy * (percentsCompletedNow - percentsCompletedLast);

            if (space == Space.World)
                tr.position += deltaMove;
            else if (space == Space.Local)
                tr.localPosition += deltaMove;
            else if (space == Space.Self)
                tr.localPosition += DuMath.RotatePoint(deltaMove, tr.localEulerAngles);
        }
    }
}
