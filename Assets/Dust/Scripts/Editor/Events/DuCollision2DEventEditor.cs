using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCollision2DEvent))]
    [CanEditMultipleObjects]
    public class DuCollision2DEventEditor : DuColliderEventEditor
    {
        [MenuItem("Dust/Events/On Collision 2D")]
        public static void AddComponent()
        {
            AddComponentToSelectedOrNewObject("DuCollision2D", typeof(DuCollision2DEvent));
        }
    }
}
