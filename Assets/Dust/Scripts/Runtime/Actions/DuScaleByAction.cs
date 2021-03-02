using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/ScaleBy Action")]
    public class DuScaleByAction : DuScaleAction
    {
        [SerializeField]
        private Vector3 m_ScaleBy = Vector3.one;
        public Vector3 scaleBy
        {
            get => m_ScaleBy;
            set => m_ScaleBy = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        protected override void OnActionStart()
        {
            base.OnActionStart();

            if (Dust.IsNull(m_TargetTransform))
                return;

            if (space == Space.World)
                m_ScaleStart = m_TargetTransform.lossyScale;
            else if (space == Space.Local)
                m_ScaleStart = m_TargetTransform.localScale;

            m_ScaleFinal = Vector3.Scale(m_ScaleStart, scaleBy);
            
            // for 1st Update I should decrease Next value for current scale-value
            m_ScaleLast = m_ScaleStart;
        }
    }
}
