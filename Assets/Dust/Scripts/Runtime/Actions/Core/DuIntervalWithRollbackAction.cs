using UnityEngine;

namespace DustEngine
{
    public abstract class DuIntervalWithRollbackAction : DuAction
    {
        public enum PlayingPhase
        {
            Idle = 0,
            Main = 1,
            Rollback = 2
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private float m_Duration = 1f;
        public float duration
        {
            get => m_Duration;
            set => m_Duration = Normalizer.Duration(value);
        }
        
        [SerializeField]
        private bool m_PlayRollback = false;
        public bool playRollback
        {
            get => m_PlayRollback;
            set => m_PlayRollback = value;
        }

        [SerializeField]
        private float m_RollbackDuration = 1f;
        public float rollbackDuration
        {
            get => m_RollbackDuration;
            set => m_RollbackDuration = Normalizer.Duration(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private PlayingPhase m_PlayingPhase = PlayingPhase.Idle;
        public PlayingPhase playingPhase => m_PlayingPhase;

        private float m_PlaybackStateInPhase;
        public float playbackStateInPhase => m_PlaybackStateInPhase;

        private float m_PreviousStateInPhase;
        protected float previousStateInPhase => m_PreviousStateInPhase;

        //--------------------------------------------------------------------------------------------------------------

        internal override void ActionInnerStart(DuAction previousAction)
        {
            m_PlaybackStateInPhase = 0f;
            m_PreviousStateInPhase = 0f;
            m_PlayingPhase = PlayingPhase.Main;

            base.ActionInnerStart(previousAction);
        }

        internal override void ActionInnerStop(bool isTerminated)
        {
            m_PlaybackStateInPhase = 0f;
            m_PreviousStateInPhase = 0f;
            m_PlayingPhase = PlayingPhase.Idle;

            base.ActionInnerStop(isTerminated);
        }

        internal override void ActionInnerUpdate(float deltaTime)
        {
            float curDuration = playingPhase == PlayingPhase.Main ? duration : rollbackDuration;

            if (curDuration > 0f)
            {
                m_PreviousStateInPhase = m_PlaybackStateInPhase;
                m_PlaybackStateInPhase = Mathf.Min(m_PlaybackStateInPhase + deltaTime / curDuration, 1f);
            }
            else
            {
                m_PreviousStateInPhase = 0f;
                m_PlaybackStateInPhase = 1f;
            }

            OnActionUpdate(deltaTime);

            if (m_PlaybackStateInPhase >= 1f)
            {
                if (playRollback && playingPhase == PlayingPhase.Main)
                {
                    m_PlaybackStateInPhase = 0f;
                    m_PreviousStateInPhase = 0f;
                    m_PlayingPhase = PlayingPhase.Rollback;
                }
                else
                {
                    ActionInnerStop(false);
                }
            }
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
