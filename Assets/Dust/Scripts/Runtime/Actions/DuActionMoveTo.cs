using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Action MoveTo")]
    public class DuActionMoveTo : DuIntervalAction
    {
        public enum EndPointSpace
        {
            Local = 0,
            World = 1,
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
        private EndPointSpace m_EndPointSpace = EndPointSpace.Local;
        public EndPointSpace endPointSpace
        {
            get => m_EndPointSpace;
            set => m_EndPointSpace = value;
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

            switch (endPointSpace)
            {
                case EndPointSpace.Local:
                    tr.localPosition = Vector3.Lerp(tr.localPosition, endPoint, lerpOffset);
                    break;

                case EndPointSpace.World:
                    tr.position = Vector3.Lerp(tr.position, endPoint, lerpOffset);
                    break;
            }
        }
    }
}
