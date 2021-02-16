using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    public abstract class DuAction : DuMonoBehaviour
    {
        public enum TargetMode
        {
            Self = 0,
            Parent = 1,
            GameObject = 2
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_AutoStart = false;
        public bool autoStart
        {
            get => m_AutoStart;
            set => m_AutoStart = value;
        }

        [SerializeField]
        private TargetMode m_TargetMode = TargetMode.Self;
        public TargetMode targetMode
        {
            get => m_TargetMode;
            set => m_TargetMode = value;
        }

        [SerializeField]
        private GameObject m_TargetObject = null;
        public GameObject targetObject
        {
            get => m_TargetObject;
            set => m_TargetObject = value;
        }

        [SerializeField]
        private List<DuAction> m_OnComplete = null;
        public List<DuAction> onComplete
        {
            get
            {
                if (Dust.IsNull(m_OnComplete))
                    m_OnComplete = new List<DuAction>();

                return m_OnComplete;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected bool m_IsPlaying;
        public bool isPlaying => m_IsPlaying;

        protected float m_PercentsCompletedLast;
        public float percentsCompletedLast => m_PercentsCompletedLast;

        protected float m_PercentsCompletedNow;
        public float percentsCompletedNow => m_PercentsCompletedNow;

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            if (autoStart)
                Play();
        }

        private void Update()
        {
            if (!isPlaying)
                return;

            ActionInnerUpdate(Time.deltaTime);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Play()
        {
            if (isPlaying)
                return;

            ActionInnerStart();
        }

        public void Stop()
        {
            if (!isPlaying)
                return;

            ActionInnerStop(true);
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle INNER

        internal void ActionInnerStart()
        {
            m_IsPlaying = true;

            m_PercentsCompletedLast = 0f;
            m_PercentsCompletedNow = 0f;

            OnActionStart();
        }

        internal abstract void ActionInnerUpdate(float deltaTime);

        internal void ActionInnerStop(bool isTerminated)
        {
            m_IsPlaying = false;

            m_PercentsCompletedLast = 0f;
            m_PercentsCompletedNow = 0f;

            OnActionStop(isTerminated);

            if (!isTerminated && Dust.IsNotNull(onComplete))
            {
                for (int i = 0; i < onComplete.Count; i++)
                {
                    onComplete[i].Play();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal virtual void OnActionStart()
        {
            // Initialize data if required
        }

        internal abstract void OnActionUpdate(float deltaTime);

        internal virtual void OnActionStop(bool isTerminated)
        {
            // Release data if required
        }

        //--------------------------------------------------------------------------------------------------------------

        protected Transform GetTargetTransform()
        {
            switch (targetMode)
            {
                case TargetMode.Self:
                    return transform;

                case TargetMode.Parent:
                    return transform.parent;

                case TargetMode.GameObject:
                    return targetObject.transform;

                default:
                    return null;
            }
        }
    }
}
