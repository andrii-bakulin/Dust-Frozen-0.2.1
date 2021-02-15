using System;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Action Callback")]
    public class DuActionCallback : DuInstantAction
    {
        [Serializable]
        public class ActionCallback : UnityEvent<DuAction>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ActionCallback m_Callback = null;
        public ActionCallback callback => m_Callback;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionUpdate(float deltaTime)
        {
            callback?.Invoke(this);
        }
    }
}
