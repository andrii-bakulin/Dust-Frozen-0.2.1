using UnityEngine;

namespace DustEngine
{
    public abstract class DuIntervalAction : DuAction
    {
        [SerializeField]
        private float m_Duration = 1f;
        public float duration
        {
            get => m_Duration;
            set => m_Duration = Normalizer.Duration(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private float m_PlaybackState;
        public float playbackState => m_PlaybackState;

        private float m_PreviousState;
        protected float previousState => m_PreviousState;

        //--------------------------------------------------------------------------------------------------------------

        internal override void ActionInnerStart(DuAction previousAction)
        {
            m_PreviousState = 0f;
            m_PlaybackState = 0f;

            base.ActionInnerStart(previousAction);
        }

        internal override void ActionInnerStop(bool isTerminated)
        {
            m_PreviousState = 0f;
            m_PlaybackState = 0f;

            base.ActionInnerStop(isTerminated);
        }

        internal override void ActionInnerUpdate(float deltaTime)
        {
            if (duration > 0f)
            {
                m_PreviousState = m_PlaybackState;
                m_PlaybackState = Mathf.Min(m_PlaybackState + deltaTime / duration, 1f);
            }
            else
            {
                m_PreviousState = 0f;
                m_PlaybackState = 1f;
            }

            OnActionUpdate(deltaTime);

            if (m_PlaybackState >= 1f)
                ActionInnerStop(false);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static float Duration(float value)
            {
                return Mathf.Max(value, 0f);
            }
        }
    }
}
