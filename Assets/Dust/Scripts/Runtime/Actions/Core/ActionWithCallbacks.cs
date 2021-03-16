using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    public abstract class ActionWithCallbacks : Action
    {
        [Serializable]
        public class ActionCallback : UnityEvent<Action>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected ActionCallback m_OnCompleteCallback = null;
        public ActionCallback onCompleteCallback => m_OnCompleteCallback;

        [SerializeField]
        protected List<Action> m_OnCompleteActions = null;
        public List<Action> onCompleteActions
        {
            get
            {
                if (Dust.IsNull(m_OnCompleteActions))
                    m_OnCompleteActions = new List<Action>();

                return m_OnCompleteActions;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void ActionInnerStop(bool isTerminated)
        {
            base.ActionInnerStop(isTerminated);
            
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
    }
}
