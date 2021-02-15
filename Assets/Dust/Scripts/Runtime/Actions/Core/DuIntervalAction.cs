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

        //--------------------------------------------------------------------------------------------------------------

        internal override void ActionInnerUpdate(float deltaTime)
        {
            if (duration > 0f)
            {
                m_PercentsCompletedLast = m_PercentsCompletedNow;
                m_PercentsCompletedNow = Mathf.Min(m_PercentsCompletedNow + deltaTime / duration, 1f);
            }
            else
            {
                m_PercentsCompletedLast = 0f;
                m_PercentsCompletedNow = 1f;
            }

            OnActionUpdate(deltaTime);

            if (m_PercentsCompletedNow >= 1f)
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
