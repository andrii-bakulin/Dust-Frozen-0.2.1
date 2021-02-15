using DustEngine.DustEditor.UI;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public abstract class DuActionEditor : DuEditor
    {
        protected DuProperty m_AutoStart;

        protected DuProperty m_TargetMode;
        protected DuProperty m_TargetObject;

        protected DuProperty m_OnCompete;

        //--------------------------------------------------------------------------------------------------------------

        protected DuAction.TargetMode targetMode => (DuAction.TargetMode) m_TargetMode.enumValueIndex;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_AutoStart = FindProperty("m_AutoStart", "Auto Start");

            m_TargetMode = FindProperty("m_TargetMode", "Target Mode");
            m_TargetObject = FindProperty("m_TargetObject", "Target Object");

            m_OnCompete = FindProperty("m_OnCompete", "On Complete Start Next Actions");
        }

        //--------------------------------------------------------------------------------------------------------------

        private Rect m_RectsAddButton;

        protected void OnInspectorGUI_BaseControlUI()
        {
            if (targets.Length != 1)
                return;

            var isAutoStart = m_AutoStart.IsTrue;
            var duAction = target as DuAction;

            string iconName;
            string iconTitle;

            if (Application.isPlaying)
            {
                iconName  = duAction.isPlaying ? Icons.ACTION_PLAY : Icons.ACTION_IDLE;
                iconTitle = duAction.isPlaying ? "Playing" : "Idle";
            }
            else
            {
                iconName  = isAutoStart ? Icons.ACTION_PLAY : Icons.ACTION_IDLE;
                iconTitle = isAutoStart ? "Auto Start" : "Idle";
            }

            DustGUI.BeginHorizontal();
            {
                if (DustGUI.IconButton(iconName))
                {
                    if (Application.isPlaying)
                    {
                        if (!duAction.isPlaying)
                            duAction.Play();
                        else
                            duAction.Stop();
                    }
                    else
                    {
                        m_AutoStart.valBool = !m_AutoStart.valBool;
                        m_AutoStart.isChanged = true;
                    }
                }

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DustGUI.BeginVertical();
                {
                    DustGUI.SimpleLabel(iconTitle);

                    var rect = EditorGUILayout.GetControlRect(false, 8f);
                    EditorGUI.ProgressBar(rect, duAction.percentsCompletedNow, "");
                }
                DustGUI.EndVertical();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DustGUI.Lock();
                DustGUI.IconButton(Icons.ACTION_NEXT, DustGUI.Config.ICON_BUTTON_WIDTH * 0.75f, DustGUI.Config.ICON_BUTTON_HEIGHT);
                DustGUI.Unlock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                foreach (DuAction nextAction in duAction.onCompete)
                {
                    if (Dust.IsNull(nextAction))
                        continue;

                    Texture icon = Icons.GetTextureByComponent(nextAction);
                    DustGUI.IconButton(icon);
                }

                if (DustGUI.IconButton(Icons.ACTION_ADD_ACTION))
                    PopupWindow.Show(m_RectsAddButton, DuActionsPopupButtons.Popup(duAction));

                if (Event.current.type == EventType.Repaint)
                    m_RectsAddButton = GUILayoutUtility.GetLastRect();

                // @todo: make in future catch "Drag-n-Drop" event with other action in __this__ object and append it
                //        too the actions list
            }
            DustGUI.EndHorizontal();

            if (Application.isPlaying)
                DustGUI.ForcedRedrawInspector(this);
        }

        protected void OnInspectorGUI_AnyActionFields(string actionId)
        {
            PropertyField(m_OnCompete);

            if (DustGUI.FoldoutBegin("Extended", actionId + ".Extended", false))
            {
                PropertyField(m_AutoStart);

                PropertyField(m_TargetMode);
                PropertyFieldOrHide(m_TargetObject, targetMode != DuAction.TargetMode.GameObject);
            }
            DustGUI.FoldoutEnd();
        }
    }
}
