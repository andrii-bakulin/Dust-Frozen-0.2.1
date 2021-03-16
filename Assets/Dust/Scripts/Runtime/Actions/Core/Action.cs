using UnityEngine;

namespace DustEngine
{
    public abstract class Action : DuMonoBehaviour
    {
        public enum TargetMode
        {
            Self = 0,
            ParentObject = 1,
            GameObject = 2,
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
        protected TargetMode m_TargetMode = TargetMode.Self;
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

        private GameObject m_ActiveTargetObject;
        public GameObject activeTargetObject => m_ActiveTargetObject;

        private Transform m_ActiveTargetTransform;
        public Transform activeTargetTransform => m_ActiveTargetTransform;

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

        public void Play(Action previousAction)
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
            var actions = target.GetComponents<Action>();

            foreach (var action in actions)
            {
                if (action.isPlaying)
                    action.Stop();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle INNER

        protected virtual void ActionInnerStart(Action previousAction)
        {
            if (Dust.IsNotNull(previousAction))
            {
                // Continue execution
                m_ActiveTargetObject = previousAction.activeTargetObject;
            }
            else
            {
                // Start Action
                // Target object is SELF by default
                m_ActiveTargetObject = gameObject;
                
                // Only for auto-start action user able to assign target object
                if (autoStart) switch (targetMode)
                {
                    case TargetMode.Self:
                        // nothing need to change
                        break;

                    case TargetMode.ParentObject:
                        m_ActiveTargetObject = Dust.IsNotNull(transform.parent) ? transform.parent.gameObject : null;
                        break;

                    case TargetMode.GameObject:
                        m_ActiveTargetObject = targetObject;
                        break;
                }
            }

            if (Dust.IsNull(activeTargetObject))
            {
                Debug.LogError("Cannot start action, because failed to detect target object");
                return;
            }
            
            m_ActiveTargetTransform = activeTargetObject.transform;

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
        // Dust.Action lifecycle

        protected virtual void OnActionStart()
        {
            // Nothing need to do
        }

        protected abstract void OnActionUpdate(float deltaTime);

        protected virtual void OnActionStop(bool isTerminated)
        {
            // Nothing need to do
        }
    }
}
