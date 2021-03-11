using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuUpdateHierarchyAction))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class DuUpdateHierarchyActionEditor : DuInstantActionEditor
    {
        private DuProperty m_UpdateMode;
        private DuProperty m_OrderMode;
        private DuProperty m_ReferenceObject;

        private DuUpdateHierarchyAction.UpdateMode updateMode
            => (DuUpdateHierarchyAction.UpdateMode) m_UpdateMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        static DuUpdateHierarchyActionEditor()
        {
            DuActionsPopupButtons.AddActionTransform(typeof(DuUpdateHierarchyAction), "Update Hierarchy");
        }

        [MenuItem("Dust/Actions/Update Hierarchy")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Update Hierarchy Action", typeof(DuUpdateHierarchyAction));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_UpdateMode = FindProperty("m_UpdateMode", "Update Mode");
            m_OrderMode = FindProperty("m_OrderMode", "Order Mode");
            m_ReferenceObject = FindProperty("m_ReferenceObject", "Reference Object");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            OnInspectorGUI_BaseControlUI();

            if (DustGUI.FoldoutBegin("Parameters", "DuUpdateHierarchyAction.Parameters"))
            {
                PropertyField(m_UpdateMode);

                if (updateMode == DuUpdateHierarchyAction.UpdateMode.SetTargetAsChildOfReferenceObject ||
                    updateMode == DuUpdateHierarchyAction.UpdateMode.SetReferenceObjectAsChildOfTarget)
                {
                    PropertyField(m_OrderMode);
                }

                PropertyField(m_ReferenceObject);
            }
            DustGUI.FoldoutEnd();

            OnInspectorGUI_Callbacks("DuUpdateHierarchyAction");
            OnInspectorGUI_Extended("DuUpdateHierarchyAction");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();
        }
    }
}
