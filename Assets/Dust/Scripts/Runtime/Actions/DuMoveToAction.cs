using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/MoveTo Action")]
    public class DuMoveToAction : DuMoveAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_MoveTo = Vector3.zero;
        public Vector3 moveTo
        {
            get => m_MoveTo;
            set => m_MoveTo = value;
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

            Vector3 localMoveTo;

            if (space == Space.World)
            {
                localMoveTo = Dust.IsNotNull(tr.parent) ? tr.parent.InverseTransformPoint(m_MoveTo) : m_MoveTo;
            }
            else if (space == Space.Local)
            {
                localMoveTo = m_MoveTo;
            }
            else return;

            m_DeltaLocalMove = localMoveTo - tr.localPosition; 
        }
    }
}
