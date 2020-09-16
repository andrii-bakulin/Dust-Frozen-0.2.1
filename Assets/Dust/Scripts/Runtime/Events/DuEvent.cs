using System;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuEvent : DuMonoBehaviour
    {
#if UNITY_EDITOR
        protected static DuEvent AddComponentByEventType(Type eventType)
        {
            if (Dust.IsNull(Selection.activeGameObject))
                return null;

            return Selection.activeGameObject.AddComponent(eventType) as DuEvent;
        }
#endif
    }
}
