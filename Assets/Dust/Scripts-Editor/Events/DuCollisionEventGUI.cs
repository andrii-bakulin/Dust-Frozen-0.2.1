using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCollisionEvent)), CanEditMultipleObjects]
    public class DuCollisionEventGUI : DuColliderEventGUI
    {
    }
}
#endif
