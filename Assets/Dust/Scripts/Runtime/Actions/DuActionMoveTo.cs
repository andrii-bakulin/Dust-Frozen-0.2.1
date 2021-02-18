using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Action MoveTo")]
    public class DuActionMoveTo : DuIntervalAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_EndPoint = Vector3.zero;
        public Vector3 endPoint
        {
            get => m_EndPoint;
            set => m_EndPoint = value;
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

            var lerpOffset = duration > 0f && percentsCompletedNow < 1f
                ? deltaTime / ((1f - percentsCompletedNow) * duration)
                : 1f;

            switch (space)
            {
                case Space.World:
                    tr.position = Vector3.Lerp(tr.position, endPoint, lerpOffset);
                    break;

                case Space.Local:
                    tr.localPosition = Vector3.Lerp(tr.localPosition, endPoint, lerpOffset);
                    break;
            }
        }
    }
}
