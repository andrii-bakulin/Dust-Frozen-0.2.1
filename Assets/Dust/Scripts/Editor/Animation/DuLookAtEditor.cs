using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuLookAt))]
    [CanEditMultipleObjects]
    public class DuLookAtEditor : DuEditor
    {
        private DuProperty m_TargetObject;
        private DuProperty m_UpVectorObject;

        private DuProperty m_UpdateMode;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Animation/LookAt")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("LookAt", typeof(DuLookAt));
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            m_TargetObject = FindProperty("m_TargetObject", "Target Object");
            m_UpVectorObject = FindProperty("m_UpVectorObject", "Up Vector Object");

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_TargetObject);
            PropertyField(m_UpVectorObject);

            Space();

            PropertyField(m_UpdateMode);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
