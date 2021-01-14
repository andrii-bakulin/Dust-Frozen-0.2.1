using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTriggerEvent))]
    [CanEditMultipleObjects]
    public class DuTriggerEventEditor : DuColliderEventEditor
    {
        [MenuItem("Dust/Events/On Trigger")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuTrigger", typeof(DuTriggerEvent));
        }
    }
}
