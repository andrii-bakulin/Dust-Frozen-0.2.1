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

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();
        }

        [MenuItem("Dust/Fields/Math Fields/Invert")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuInvertField));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Parameters", "DuAnyField.Parameters"))
            {
                PropertyField(m_CustomHint);
                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
