using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuSphereField))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuSphereFieldEditor : DuObjectFieldEditor
    {
        private DuProperty m_Radius;

        //--------------------------------------------------------------------------------------------------------------

        static DuSphereFieldEditor()
        {
            DuPopupButtons.AddObjectField(typeof(DuSphereField), "Sphere");
        }

        [MenuItem("Dust/Fields/Object Fields/Sphere")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuSphereField));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            OnEnableField();

            m_Radius = FindProperty("m_Radius", "Radius");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Field Parameters", "DuSphereField.Params"))
            {
                PropertyExtendedSlider(m_Radius, 0f, 10f, 0.01f);
                Space();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_RemappingBlock();
            OnInspectorGUI_GizmoBlock();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (m_Radius.isChanged)
                m_Radius.valFloat = DuSphereField.ShapeNormalizer.Radius(m_Radius.valFloat);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
