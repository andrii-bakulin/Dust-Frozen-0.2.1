using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public abstract class DuEventEditor : DuEditor
    {
        public override void OnInspectorGUI()
        {
        }
    }
}
#endif
