using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuInvertField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuInvertFieldEditor : DuFieldEditor
    {
        static DuInvertFieldEditor()
        {
            DuPopupButtons.AddMathField(typeof(DuInvertField), "Invert");
        }

        void OnEnable()
        {
            OnEnableField();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            DustGUI.HelpBoxInfo("No parameters");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
