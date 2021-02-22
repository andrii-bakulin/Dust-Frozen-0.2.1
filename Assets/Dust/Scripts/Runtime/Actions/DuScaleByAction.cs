using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/ScaleBy Action")]
    public class DuScaleByAction : DuIntervalAction
    {
        [SerializeField]
        private Vector3 m_ScaleBy = Vector3.one;
        public Vector3 scaleBy
        {
            get => m_ScaleBy;
            set => m_ScaleBy = value;
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
            scaleFinal = Vector3.Scale(scaleStart, scaleBy);
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
