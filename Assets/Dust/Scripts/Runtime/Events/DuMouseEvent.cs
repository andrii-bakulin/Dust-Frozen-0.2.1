using System;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    [AddComponentMenu("Dust/Events/On Mouse Event")]
    public class DuMouseEvent : DuEvent
    {
        public enum MouseButtonIndex
        {
            Button0 = 0,
            Button1 = 1
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private MouseButtonIndex m_MouseButtonIndex = MouseButtonIndex.Button0;
        public MouseButtonIndex mouseButtonIndex
        {
            get => m_MouseButtonIndex;
            set => m_MouseButtonIndex = value;
        }

        [SerializeField]
        private MouseButtonEvent m_OnMouseButtonDown = null;
        public MouseButtonEvent onMouseButtonDown => m_OnMouseButtonDown;

        [SerializeField]
        private MouseButtonEvent m_OnMouseButtonHold = null;
        public MouseButtonEvent onMouseButtonHold => m_OnMouseButtonHold;

        [SerializeField]
        private MouseButtonEvent m_OnMouseButtonUp = null;
        public MouseButtonEvent onMouseButtonUp => m_OnMouseButtonUp;

        //--------------------------------------------------------------------------------------------------------------

        [Serializable]
        public class MouseButtonEvent : UnityEvent<MouseButtonIndex>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            int buttonIndex = (int) mouseButtonIndex;

            if (Input.GetMouseButtonDown(buttonIndex) && Dust.IsNotNull(onMouseButtonDown) && onMouseButtonDown.GetPersistentEventCount() > 0)
            {
                onMouseButtonDown.Invoke(mouseButtonIndex);
            }

            if (Input.GetMouseButton(buttonIndex) && Dust.IsNotNull(onMouseButtonHold) && onMouseButtonHold.GetPersistentEventCount() > 0)
            {
                onMouseButtonHold.Invoke(mouseButtonIndex);
            }

            if (Input.GetMouseButtonUp(buttonIndex) && Dust.IsNotNull(onMouseButtonUp) && onMouseButtonUp.GetPersistentEventCount() > 0)
            {
                onMouseButtonUp.Invoke(mouseButtonIndex);
            }
        }
    }
}
