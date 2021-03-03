using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    public abstract class DuAction : DuMonoBehaviour
    {
        public enum TargetMode
        {
            Self = 0,
            ParentObject = 1,
            GameObject = 2,
            Inherit = 3,
        }

        [Serializable]
        public class ActionCallback : UnityEvent<DuAction>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_AutoStart = false;
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
        private TargetMode m_TargetMode = TargetMode.Self;
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
        private GameObject m_TargetObject = null;
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

        [SerializeField]
        private ActionCallback m_OnCompleteCallback = null;
        public ActionCallback onCompleteCallback => m_OnCompleteCallback;

        [SerializeField]
        private List<DuAction> m_OnCompleteActions = null;
        public List<DuAction> onCompleteActions
        {
            get
            {
                if (Dust.IsNull(m_OnCompleteActions))
                    m_OnCompleteActions = new List<DuAction>();

                return m_OnCompleteActions;
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
                if (Dust.IsNull(previousAction))
                {
                    Dust.Debug.Error("Cannot start action [" + gameObject.name + "]->["+GetType()+"] because previousAction is null, but it required to inherit target object");
                    return;
                }

                targetObject = previousAction.GetTargetObject();
            }

            m_IsPlaying = true;

            OnActionStart();
        }

        protected abstract void ActionInnerUpdate(float deltaTime);

        protected virtual void ActionInnerStop(bool isTerminated)
        {
            m_IsPlaying = false;

            OnActionStop(isTerminated);

            if (!isTerminated)
            {
                onCompleteCallback?.Invoke(this);

                if (Dust.IsNotNull(onCompleteActions))
                {
                    for (int i = 0; i < onCompleteActions.Count; i++)
                    {
                        onCompleteActions[i]?.Play(this);
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        protected virtual void OnActionStart()
        {
            var target = GetTargetObject();

            if (Dust.IsNull(target))
                return;

            m_TargetTransform = target.transform;
        }

        protected abstract void OnActionUpdate(float deltaTime);

        protected virtual void OnActionStop(bool isTerminated)
        {
            m_TargetTransform = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        public GameObject GetTargetObject()
        {
            switch (targetMode)
            {
                case TargetMode.Self:
                    return this.gameObject;

                case TargetMode.ParentObject:
                    return Dust.IsNotNull(transform.parent) ? transform.parent.gameObject : null;

                case TargetMode.GameObject:
                case TargetMode.Inherit:
                    return this.targetObject;

                default:
                    return null;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Reset()
        {
            ResetStates();
        }

        protected virtual void ResetStates()
        {
            if (gameObject.GetComponents<DuAction>().Length == 1)
            {
                autoStart = true;
                targetMode = TargetMode.Self;
            }
            else
            {
                autoStart = false;
                targetMode = TargetMode.Inherit;
            }
        }
    }
}
