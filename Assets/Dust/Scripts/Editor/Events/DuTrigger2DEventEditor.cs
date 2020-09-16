using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTrigger2DEvent)), CanEditMultipleObjects]
    public class DuTrigger2DEventEditor : DuColliderEventEditor
    {
    }
}
#endif
