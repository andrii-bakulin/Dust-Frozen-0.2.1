using System.Linq;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [CustomEditor(typeof(DuDebug))]
    // [CanEditMultipleObjects]
    public class DuDebugEditor : DuEditor
    {
        private DuProperty m_LogInConsole;
        private DuProperty m_LogInMessageBox;

        private DuProperty m_MessagesLimit;

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_LogInConsole = FindProperty("m_LogInConsole", "Log In Console");
            m_LogInMessageBox = FindProperty("m_LogInMessageBox", "Log In Message Box");

            m_MessagesLimit = FindProperty("m_MessagesLimit", "Messages Limit");
        }

        public override void OnInspectorGUI()
        {
            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_LogInConsole);
            PropertyField(m_LogInMessageBox);

            PropertyField(m_MessagesLimit);

            if (Application.isPlaying)
            {
                Space();

                var main = target as DuDebug;
                var message = main.GetPlainLogMessages();
                var linesCount = message.Count(f => f == '\n') + 1;

                DustGUI.Header("Log Messages:");
                EditorGUILayout.TextArea(message, GUILayout.MaxHeight(15 * Mathf.Max(6, linesCount + 1)));

                Space();

                DustGUI.BeginHorizontal();
                {
                    if (DustGUI.Button("Clear", 64, 24))
                        main.ClearLogMessages();

                    if (main.logInMessageBox)
                    {
                        if (DustGUI.Button("Freeze", 64, 24))
                            main.logInMessageBox = false;
                    }
                    else
                    {
                        if (DustGUI.Button("Unfreeze", 64, 24))
                            main.logInMessageBox = true;
                    }
                }
                DustGUI.EndHorizontal();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();

            DustGUI.ForcedRedrawInspector(this);
        }
    }
}
