using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuCollision2DEvent)), CanEditMultipleObjects]
    public class DuCollision2DEventEditor : DuColliderEventEditor
    {
    }
}
#endif
