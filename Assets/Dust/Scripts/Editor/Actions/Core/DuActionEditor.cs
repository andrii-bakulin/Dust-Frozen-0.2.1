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

        //--------------------------------------------------------------------------------------------------------------

        protected DuAction.TargetMode targetMode => (DuAction.TargetMode) m_TargetMode.valInt;

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_AutoStart = FindProperty("m_AutoStart", "Auto Start");

            m_TargetMode = FindProperty("m_TargetMode", "Target Mode (?)",
                "IMPORTANT!\n" 
                + "The Target Object will only be used for the actions that are launching. "
                + "In the chain of actions, the Target Object be inherited from the previous action.");
            
            m_TargetObject = FindProperty("m_TargetObject", "Target Object");
        }

        //--------------------------------------------------------------------------------------------------------------

        private Rect m_RectsAddButton;

        protected virtual void OnInspectorGUI_BaseControlUI()
        {
            if (targets.Length != 1)
                return;

            var isAutoStart = m_AutoStart.IsTrue;
            var duAction = target as DuAction;

            if (Dust.IsNull(duAction))
                return;
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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
            
            if (duAction as DuIntervalAction is DuIntervalAction duIntervalAction)
            {
                if (duIntervalAction.repeatMode == DuIntervalAction.RepeatMode.Repeat)
                {
                    if (Application.isPlaying && duAction.isPlaying)
                        iconTitle += $" ({duIntervalAction.playbackIndex+1}/{duIntervalAction.repeatTimes})";
                    else
                        iconTitle += $", Repeat {duIntervalAction.repeatTimes}x";
                }
                else if (duIntervalAction.repeatMode == DuIntervalAction.RepeatMode.RepeatForever)
                {
                    iconTitle += ", Repeat Forever";
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

                if (duAction as DuActionWithCallbacks is DuActionWithCallbacks duActionWithCallbacks)
                {
                    foreach (var nextAction in duActionWithCallbacks.onCompleteActions)
                    {
                        if (Dust.IsNull(nextAction))
                            continue;

                        Texture icon = Icons.GetTextureByComponent(nextAction);
                        DustGUI.IconButton(icon);
                    }
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
            if (target as DuIntervalWithRollbackAction is DuIntervalWithRollbackAction intervalWithRollbackAction)
            {
                if (intervalWithRollbackAction.playingPhase == DuIntervalWithRollbackAction.PlayingPhase.Main)
                    return intervalWithRollbackAction.playbackState;
                
                if (intervalWithRollbackAction.playingPhase == DuIntervalWithRollbackAction.PlayingPhase.Rollback)
                    return 1f - intervalWithRollbackAction.playbackState;

                return 0f;
            }
            
            if (target as DuIntervalAction is DuIntervalAction intervalAction)
                return intervalAction.playbackState;

            return 0f; // For DuInstantAction <or> others > return 0f
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected virtual void OnInspectorGUI_Extended(string actionId)
        {
            if (DustGUI.FoldoutBegin("Extended", actionId + ".Extended", this))
            {
                OnInspectorGUI_Extended_BlockFirst();
                OnInspectorGUI_Extended_BlockMiddle();
                OnInspectorGUI_Extended_BlockLast();
            }
            DustGUI.FoldoutEnd();
        }

        protected virtual void OnInspectorGUI_Extended_BlockFirst()
        {
            PropertyField(m_AutoStart);
        }

        protected virtual void OnInspectorGUI_Extended_BlockMiddle()
        {
        }

        protected virtual void OnInspectorGUI_Extended_BlockLast()
        {
            PropertyField(m_TargetMode);
            PropertyFieldOrHide(m_TargetObject, targetMode != DuAction.TargetMode.GameObject);
        }
    }
}
