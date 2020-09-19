using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCollisionEvent)), CanEditMultipleObjects]
    public class DuCollisionEventEditor : DuColliderEventEditor
    {
    }
}
