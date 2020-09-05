using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTriggerEvent)), CanEditMultipleObjects]
    public class DuTriggerEventEditor : DuColliderEventEditor
    {
    }
}
#endif
