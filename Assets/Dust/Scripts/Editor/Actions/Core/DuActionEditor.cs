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

        protected DuProperty m_OnCompleteCallback;
        protected DuProperty m_OnCompleteActions;

        //--------------------------------------------------------------------------------------------------------------

        protected DuAction.TargetMode targetMode => (DuAction.TargetMode) m_TargetMode.enumValueIndex;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_AutoStart = FindProperty("m_AutoStart", "Auto Start");

            m_TargetMode = FindProperty("m_TargetMode", "Target Mode");
            m_TargetObject = FindProperty("m_TargetObject", "Target Object");

            m_OnCompleteCallback = FindProperty("m_OnCompleteCallback", "Callback");
            m_OnCompleteActions = FindProperty("m_OnCompleteActions", "On Complete Start Actions");
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void PropertyDurationSlider(DuProperty durationProperty)
        {
            PropertyExtendedSlider(durationProperty, 0.00f, 10.0f, +0.01f, 0.00f);
        }

        //--------------------------------------------------------------------------------------------------------------

        private Rect m_RectsAddButton;

        protected void OnInspectorGUI_BaseControlUI()
        {
            if (targets.Length != 1)
                return;

            var isAutoStart = m_AutoStart.IsTrue;
            var duAction = target as DuAction;

            if (Dust.IsNull(duAction))
                return;
            
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
                    EditorGUI.ProgressBar(rect, GetActionPercentsDone(duAction), "");
                }
                DustGUI.EndVertical();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                DustGUI.Lock();
                DustGUI.IconButton(Icons.ACTION_NEXT, DustGUI.Config.ICON_BUTTON_WIDTH * 0.75f, DustGUI.Config.ICON_BUTTON_HEIGHT);
                DustGUI.Unlock();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                foreach (DuAction nextAction in duAction.onCompleteActions)
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

        protected float GetActionPercentsDone(DuAction duAction)
        {
            if (target as DuIntervalAction is DuIntervalAction intervalAction)
                return intervalAction.playbackState;
            
            return 0f; // For DuInstantAction <or> others > return 0f
        }
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected void OnInspectorGUI_AnyActionFields(string actionId)
            => OnInspectorGUI_AnyActionFields(actionId, false);

        protected void OnInspectorGUI_AnyActionFields(string actionId, bool callbackExpanded)
        {
            if (DustGUI.FoldoutBegin("On Complete Callback", actionId + ".Callback", this, callbackExpanded))
            {
                PropertyField(m_OnCompleteCallback);
            }
            DustGUI.FoldoutEnd();

            PropertyField(m_OnCompleteActions, $"{m_OnCompleteActions.title} ({m_OnCompleteActions.property.arraySize})");

            if (DustGUI.FoldoutBegin("Extended", actionId + ".Extended", this, false))
            {
                PropertyField(m_AutoStart);

                // Cannot hide this field even for actions without real target (callback, delay, ...)
                // User should be able to change target any time (for ex. to "Inherit")
                PropertyField(m_TargetMode);
                PropertyFieldOrHide(m_TargetObject, targetMode != DuAction.TargetMode.GameObject);
            }
            DustGUI.FoldoutEnd();
        }
    }
}
