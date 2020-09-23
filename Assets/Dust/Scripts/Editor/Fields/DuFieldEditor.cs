using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuFieldEditor : DuEditor
    {
        protected virtual void OnEnableField()
        {
        }

        public override void OnInspectorGUI()
        {
            // Hide default OnInspectorGUI() call
            // Extend all-fields-view if need in future...
        }
    }
}
