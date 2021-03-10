using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class DuActionsPopupButtons : DuPopupButtons
    {
        private DuAction m_DuAction;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddActionAnimate(System.Type type, string title)
        {
            AddEntity("Actions.Animate", type, title);
        }

        public static void AddActionFlow(System.Type type, string title)
        {
            AddEntity("Actions.Flow", type, title);
        }

        public static void AddActionTransform(System.Type type, string title)
        {
            AddEntity("Actions.Transform", type, title);
        }

        public static void AddActionOthers(System.Type type, string title)
        {
            AddEntity("Actions.Others", type, title);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static DuPopupButtons Popup(DuAction duAction)
        {
            var popup = new DuActionsPopupButtons();
            popup.m_DuAction = duAction;

            GenerateColumn(popup, "Actions.Animate", "Animate");
            GenerateColumn(popup, "Actions.Flow", "Flow");
            GenerateColumn(popup, "Actions.Transform", "Transform");
            GenerateColumn(popup, "Actions.Others", "Others");

            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override bool OnButtonClicked(CellRecord cellRecord)
        {
            Component newAction = Undo.AddComponent(m_DuAction.gameObject, cellRecord.type);
            
            if (m_DuAction as DuActionWithCallbacks is DuActionWithCallbacks duActionWithCallbacks)
                duActionWithCallbacks.onCompleteActions.Add((DuAction)newAction);

            return true;
        }
    }
}
