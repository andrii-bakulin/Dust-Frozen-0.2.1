using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DustEngine
{
    public abstract class DuColliderEvent : DuEvent
    {
        public enum TagProcessingMode
        {
            Contains = 0,
            NotContains = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [Serializable]
        public class CollisionEvent : UnityEvent<Collision>
        {
        }

        [Serializable]
        public class TriggerEvent : UnityEvent<Collider>
        {
        }

        [Serializable]
        public class Collision2DEvent : UnityEvent<Collision2D>
        {
        }

        [Serializable]
        public class Trigger2DEvent : UnityEvent<Collider2D>
        {
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        protected List<string> m_ObjectTags = new List<string>();
        public List<string> objectTags => m_ObjectTags;

        [SerializeField]
        private TagProcessingMode m_TagProcessingMode = TagProcessingMode.Contains;
        public TagProcessingMode tagProcessingMode
        {
            get => m_TagProcessingMode;
            set => m_TagProcessingMode = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected bool IsRequireSendEvent(GameObject otherGameObject)
        {
            if (Dust.IsNull(objectTags) || objectTags.Count == 0)
                return true;

            switch (tagProcessingMode)
            {
                case TagProcessingMode.Contains:
                    return objectTags.Contains(otherGameObject.tag);

                case TagProcessingMode.NotContains:
                    return !objectTags.Contains(otherGameObject.tag);
            }

            return false;
        }
    }
}
