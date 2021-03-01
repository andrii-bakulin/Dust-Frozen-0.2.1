using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    public abstract class DuScaleAction : DuIntervalWithRollbackAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected Vector3 m_ScaleStart;
        protected Vector3 m_ScaleFinal;
        
        protected Vector3 m_ScaleLast;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle
        
        internal override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TargetTransform))
                return;

            var scaleNext = playingPhase == PlayingPhase.Main
                ? Vector3.Lerp(m_ScaleStart, m_ScaleFinal, playbackStateInPhase)
                : Vector3.Lerp(m_ScaleFinal, m_ScaleStart, playbackStateInPhase);

            var scaleDiff = scaleNext - m_ScaleLast;

            if (space == Space.World)
            {
                DuTransform.SetGlobalScale(m_TargetTransform, m_TargetTransform.lossyScale + scaleDiff);
            }
            else if (space == Space.Local)
            {
                m_TargetTransform.localScale += scaleDiff;
            }

            m_ScaleLast = scaleNext;
        }
    }
}
