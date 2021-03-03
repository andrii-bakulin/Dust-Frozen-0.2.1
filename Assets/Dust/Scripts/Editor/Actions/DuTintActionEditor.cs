using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuTintAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuTintActionEditor : DuIntervalWithRollbackActionEditor
    {
        private DuProperty m_TintColor;
        private DuProperty m_PropertyName;

        //--------------------------------------------------------------------------------------------------------------

        static DuTintActionEditor()
        {
            DuActionsPopupButtons.AddActionAnimate(typeof(DuTintAction), "Tint");
        }

        [MenuItem("Dust/Actions/Tint")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Tint Action", typeof(DuTintAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_TintColor = FindProperty("m_TintColor", "Tint Color");
            m_PropertyName = FindProperty("m_PropertyName", "Property Name");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuTintAction.Parameters"))
            {
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Check MeshRenderer on target object

                if (targetMode != DuAction.TargetMode.Inherit)
                {
                    if (target as DuAction is DuAction duAction)
                    {
                        var gameObject = duAction.GetTargetObject();

                        if (Dust.IsNull(gameObject) || Dust.IsNull(gameObject.GetComponent<MeshRenderer>()))
                        {
                            DustGUI.HelpBoxWarning("Target Object has no MeshRenderer component");
                        }
                    }
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                PropertyField(m_TintColor);
                PropertyField(m_PropertyName);
                OnInspectorGUI_Durations();
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuTintAction");
            OnInspectorGUI_Extended("DuTintAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
