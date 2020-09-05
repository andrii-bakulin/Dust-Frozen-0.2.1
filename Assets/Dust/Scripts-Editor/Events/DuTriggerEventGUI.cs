using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTriggerEvent)), CanEditMultipleObjects]
    public class DuTriggerEventGUI : DuColliderEventGUI
    {
    }
}
#endif
