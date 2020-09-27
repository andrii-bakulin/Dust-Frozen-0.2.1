using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuColliderEventEditor : DuEventEditor
    {
        protected DuProperty m_ObjectTags;
        protected DuProperty m_TagProcessingMode;

        private DuProperty m_OnEnter;
        private DuProperty m_OnStay;
        private DuProperty m_OnExit;

        void OnEnable()
        {
            m_ObjectTags = FindProperty("m_ObjectTags", "Object Tags");
            m_TagProcessingMode = FindProperty("m_TagProcessingMode", "Processing Mode");

            m_OnEnter = FindProperty("m_OnEnter", "On Enter");
            m_OnStay = FindProperty("m_OnStay", "On Stay");
            m_OnExit = FindProperty("m_OnExit", "On Exit");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (DustGUI.FoldoutBegin("Tags", "DuColliderEvent.Tags"))
            {
                PropertyField(m_TagProcessingMode);
                PropertyField(m_ObjectTags);
                Space();
            }
            DustGUI.FoldoutEnd();

            Space();

            if (DustGUI.FoldoutBegin("Events", "DuColliderEvent.Events"))
            {
                var titleOnEnter = "On Enter" + (m_OnEnter.valUnityEvent.arraySize > 0 ? " (" + m_OnEnter.valUnityEvent.arraySize + ")" : "");
                var titleOnStay  = "On Stay"  + (m_OnStay.valUnityEvent.arraySize  > 0 ? " (" + m_OnStay.valUnityEvent.arraySize  + ")" : "");
                var titleOnExit  = "On Exit"  + (m_OnExit.valUnityEvent.arraySize  > 0 ? " (" + m_OnExit.valUnityEvent.arraySize  + ")" : "");

                var tabIndex = DustGUI.Toolbar("DuColliderEvent.Events", new[] {titleOnEnter, titleOnStay, titleOnExit});

                switch (tabIndex)
                {
                    case 0:
                        PropertyField(m_OnEnter);
                        break;

                    case 1:
                        PropertyField(m_OnStay);
                        break;

                    case 2:
                        PropertyField(m_OnExit);
                        break;
                }

                Space();
            }
            DustGUI.FoldoutEnd();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            serializedObject.ApplyModifiedProperties();
        }
    }
}
