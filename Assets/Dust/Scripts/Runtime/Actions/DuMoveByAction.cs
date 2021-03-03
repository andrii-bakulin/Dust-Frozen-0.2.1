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
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_MoveBy = value;
            }
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Space = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnActionStart()
        {
            base.OnActionStart();

            if (Dust.IsNull(m_TargetTransform))
                return;

            if (space == Space.World)
            {
                var trParent = m_TargetTransform.parent;
                if (Dust.IsNotNull(trParent))
                    m_DeltaLocalMove = trParent.InverseTransformPoint(moveBy) - trParent.InverseTransformPoint(Vector3.zero);
                else
                    m_DeltaLocalMove = moveBy;
            }
            else if (space == Space.Local)
            {
                m_DeltaLocalMove = moveBy;
            }
            else if (space == Space.Self)
            {
                m_DeltaLocalMove = DuMath.RotatePoint(moveBy, m_TargetTransform.localEulerAngles);
            }
        }
    }
}
