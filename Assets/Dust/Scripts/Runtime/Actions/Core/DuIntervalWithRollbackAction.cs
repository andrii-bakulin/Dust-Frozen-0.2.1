using UnityEngine;

namespace DustEngine
{
    public abstract class DuIntervalWithRollbackAction : DuIntervalAction
    {
        public enum PlayingPhase
        {
            Idle = 0,
            Main = 1,
            Rollback = 2
        }

        //--------------------------------------------------------------------------------------------------------------

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

        //--------------------------------------------------------------------------------------------------------------

        internal override void ActionPlaybackInitialize()
        {
            base.ActionPlaybackInitialize();

            m_PlayingPhase = PlayingPhase.Main;
        }

        internal override void ActionInnerUpdate(float deltaTime)
        {
            if (playRollback && DuMath.IsZero(duration) && DuMath.IsZero(rollbackDuration))
            {
                ActionInnerStop(false);
                return;
            }
            
            float curDuration = playingPhase == PlayingPhase.Main ? duration : rollbackDuration;

            if (curDuration > 0f)
            {
                m_PreviousState = m_PlaybackState;
                m_PlaybackState = Mathf.Min(m_PlaybackState + deltaTime / curDuration, 1f);
            }
            else
            {
                m_PreviousState = 0f;
                m_PlaybackState = 1f;
            }

            OnActionUpdate(deltaTime);

            if (m_PlaybackState >= 1f)
            {
                if (playRollback && playingPhase == PlayingPhase.Main)
                {
                    m_PlaybackState = 0f;
                    m_PreviousState = 0f;
                    m_PlayingPhase = PlayingPhase.Rollback;
                }
                else
                {
                    ActionPlaybackComplete();
                }
            }
        }

        internal override void ActionInnerStop(bool isTerminated)
        {
            m_PlayingPhase = PlayingPhase.Idle;

            base.ActionInnerStop(isTerminated);
        }
    }
}
