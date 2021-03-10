using UnityEngine;

namespace DustEngine
{
    public abstract class DuAction : DuMonoBehaviour
    {
        public enum TargetMode
        {
            Inherit = 0,
            Self = 1,
            ParentObject = 2,
            GameObject = 3,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected bool m_AutoStart = false;
        public bool autoStart
        {
            get => m_AutoStart;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_AutoStart = value;
            }
        }

        [SerializeField]
        protected TargetMode m_TargetMode = TargetMode.Inherit;
        public TargetMode targetMode
        {
            get => m_TargetMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TargetMode = value;
            }
        }

        [SerializeField]
        protected GameObject m_TargetObject = null;
        public GameObject targetObject
        {
            get => m_TargetObject;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TargetObject = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected Transform m_TargetTransform;

        protected bool m_IsPlaying;
        public bool isPlaying => m_IsPlaying;

        //--------------------------------------------------------------------------------------------------------------

        protected bool IsAllowUpdateProperty()
        {
            if (isPlaying)
            {
#if UNITY_EDITOR
                Dust.Debug.Warning("Cannot update property for action while it playing");
#endif
                return false;
            }

            return true;
        }

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

        public void Play() => Play(null);

        public void Play(DuAction previousAction)
        {
            if (isPlaying)
                return;

            ActionInnerStart(previousAction);
        }

        public void Stop()
        {
            if (!isPlaying)
                return;

            ActionInnerStop(true);
        }

        public void StopAllActionsAndPlay()
        {
            StopAllActions();
            Play();
        }

        public void StopAllActions()
        {
            StopAllActions(this.gameObject);
        }

        public static void StopAllActions(GameObject target)
        {
            var duActions = target.GetComponents<DuAction>();

            foreach (var duAction in duActions)
            {
                if (duAction.isPlaying)
                    duAction.Stop();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle INNER

        protected virtual void ActionInnerStart(DuAction previousAction)
        {
            if (targetMode == TargetMode.Inherit)
            {
                m_TargetObject = Dust.IsNotNull(previousAction) ? previousAction.GetTargetObject() : this.gameObject;
            }
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            
            var activeTargetObject = GetTargetObject();

            m_TargetTransform = Dust.IsNotNull(activeTargetObject) ? activeTargetObject.transform : null;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            m_IsPlaying = true;

            OnActionStart();
        }

        protected abstract void ActionInnerUpdate(float deltaTime);

        protected virtual void ActionInnerStop(bool isTerminated)
        {
            m_IsPlaying = false;

            OnActionStop(isTerminated);
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        protected virtual void OnActionStart()
        {
            // Nothing need to do
        }

        protected abstract void OnActionUpdate(float deltaTime);

        protected virtual void OnActionStop(bool isTerminated)
        {
            // Nothing need to do
        }

        //--------------------------------------------------------------------------------------------------------------

        public GameObject GetTargetObject()
        {
            switch (targetMode)
            {
                case TargetMode.Inherit:
                    return Dust.IsNotNull(this.targetObject) ? this.targetObject : this.gameObject;

                case TargetMode.Self:
                    return this.gameObject;

                case TargetMode.ParentObject:
                    return Dust.IsNotNull(transform.parent) ? transform.parent.gameObject : null;

                case TargetMode.GameObject:
                    return this.targetObject;

                default:
                    return null; 
            }
        }
    }
}
