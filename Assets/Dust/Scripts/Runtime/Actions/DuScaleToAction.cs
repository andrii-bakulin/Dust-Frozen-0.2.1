using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/ScaleTo Action")]
    public class DuScaleToAction : DuIntervalAction
    {
        [SerializeField]
        private Vector3 m_ScaleTo = Vector3.one;
        public Vector3 scaleTo
        {
            get => m_ScaleTo;
            set => m_ScaleTo = value;
        }

        private Vector3 scaleStart;
        private Vector3 scaleFinal;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionStart()
        {
            base.OnActionStart();

            Transform tr = GetTargetTransform();

            if (Dust.IsNull(tr))
                return;

            scaleStart = tr.localScale;
            scaleFinal = scaleTo;
        }

        internal override void OnActionUpdate(float deltaTime)
        {
            Transform tr = GetTargetTransform();

            if (Dust.IsNull(tr))
                return;

            tr.localScale = Vector3.Lerp(scaleStart, scaleFinal, percentsCompletedNow);
        }
    }
}
