using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/ScaleTo Action")]
    public class DuScaleToAction : DuIntervalAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_ScaleTo = Vector3.one;
        public Vector3 scaleTo
        {
            get => m_ScaleTo;
            set => m_ScaleTo = value;
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

            if (space == Space.World)
                scaleStart = tr.lossyScale;
            else if (space == Space.Local)
                scaleStart = tr.localScale;

            scaleFinal = scaleTo;
        }

        internal override void OnActionUpdate(float deltaTime)
        {
            Transform tr = GetTargetTransform();

            if (Dust.IsNull(tr))
                return;

            if (space == Space.World)
                DuTransform.SetGlobalScale(tr, Vector3.Lerp(scaleStart, scaleFinal, percentsCompletedNow));
            else if (space == Space.Local)
                tr.localScale = Vector3.Lerp(scaleStart, scaleFinal, percentsCompletedNow);
        }
    }
}
