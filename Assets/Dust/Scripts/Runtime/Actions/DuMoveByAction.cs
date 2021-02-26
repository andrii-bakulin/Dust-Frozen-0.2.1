using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/MoveBy Action")]
    public class DuMoveByAction : DuMoveAction
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

        internal override void OnActionStart()
        {
            base.OnActionStart();

            Transform tr = GetTargetTransform();

            if (Dust.IsNull(tr))
                return;

            if (space == Space.World)
            {
                if (Dust.IsNotNull(tr.parent))
                    m_DeltaLocalMove = tr.parent.InverseTransformPoint(moveBy) - tr.parent.InverseTransformPoint(Vector3.zero);
                else
                    m_DeltaLocalMove = moveBy;
            }
            else if (space == Space.Local)
            {
                m_DeltaLocalMove = moveBy;
            }
            else if (space == Space.Self)
            {
                m_DeltaLocalMove = DuMath.RotatePoint(moveBy, tr.localEulerAngles);
            }
        }
    }
}
