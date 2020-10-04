using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTriggerEvent)), CanEditMultipleObjects]
    public class DuTriggerEventEditor : DuColliderEventEditor
    {
        [MenuItem("Dust/Events/Trigger Event")]
        public static DuTriggerEvent AddComponent()
        {
            return AddComponentByEventType(typeof(DuTriggerEvent)) as DuTriggerEvent;
        }
    }
}
