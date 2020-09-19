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
            (target as DuMonoBehaviour).enabled = true; // Forced activate all field-scripts
        }
    }
}
