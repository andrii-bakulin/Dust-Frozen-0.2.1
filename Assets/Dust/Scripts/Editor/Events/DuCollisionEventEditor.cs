using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCollisionEvent))]
    [CanEditMultipleObjects]
    public class DuCollisionEventEditor : DuColliderEventEditor
    {
        [MenuItem("Dust/Events/On Collision")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuCollision", typeof(DuCollisionEvent));
        }
    }
}
