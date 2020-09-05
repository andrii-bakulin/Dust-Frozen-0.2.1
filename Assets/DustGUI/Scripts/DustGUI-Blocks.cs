using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    public static partial class DustGUI
    {
        public static Rect BeginHorizontal(float width = 0, float height = 0)
            => BeginHorizontal(GUIStyle.none, width, height);

        public static Rect BeginHorizontalBox(float width = 0, float height = 0)
            => BeginHorizontal("box", width, height);

        public static Rect BeginHorizontal(GUIStyle style, float width = 0, float height = 0)
        {
            return EditorGUILayout.BeginHorizontal(style, PackOptions(width, height));
        }

        public static void EndHorizontal()
        {
            EditorGUILayout.EndHorizontal();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Rect BeginVertical(float width = 0, float height = 0)
            => BeginVertical(GUIStyle.none, width, height);

        public static Rect BeginVerticalBox(float width = 0, float height = 0)
            => BeginVertical("box", width, height);

        public static Rect BeginVertical(GUIStyle style, float width = 0, float height = 0)
        {
            return EditorGUILayout.BeginVertical(style, PackOptions(width, height));
        }

        public static void EndVertical()
        {
            EditorGUILayout.EndVertical();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool FoldoutBegin(string title, string foldoutId)
            => FoldoutBegin(title, foldoutId, null, true);

        public static bool FoldoutBegin(string title, string foldoutId, bool defaultState)
            => FoldoutBegin(title, foldoutId, null, defaultState);

        public static bool FoldoutBegin(string title, string foldoutId, Object targetId)
            => FoldoutBegin(title, foldoutId, targetId, true);

        public static bool FoldoutBegin(string title, string foldoutId, Object targetId, bool defaultState)
        {
#if UNITY_2019_1_OR_NEWER
            string key = "DustEngine.DustGUI.Foldout." + foldoutId;

            if (targetId != null)
                key += "." + targetId.GetInstanceID();

            bool state = SessionState.GetBool(key, defaultState);
            state = EditorGUILayout.BeginFoldoutHeaderGroup(state, title);
            SessionState.SetBool(key, state);

            IndentLevelInc();
            return state;
#else
            return FoldoutBegin(title);
#endif
        }

        public static bool FoldoutBegin(string title)
        {
#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.BeginFoldoutHeaderGroup(true, title);
#else
            Header(title);
#endif

            IndentLevelInc();
            return true;
        }

        public static void FoldoutEnd()
        {
            IndentLevelDec();

#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.EndFoldoutHeaderGroup();
#else
            SpaceLine();
#endif
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector2 BeginScrollView(Vector2 scrollPosition, float width = 0, float height = 0)
            => BeginScrollView(scrollPosition, GUIStyle.none, width, height);

        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style, float width = 0, float height = 0)
        {
            return EditorGUILayout.BeginScrollView(scrollPosition, style, PackOptions(width, height));
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool BeginScrollView(ref Vector2 scrollPosition, float width = 0, float height = 0)
            => BeginScrollView(ref scrollPosition, GUIStyle.none, width, height);

        public static bool BeginScrollView(ref Vector2 scrollPosition, GUIStyle style, float width = 0, float height = 0)
        {
            var lastPosition = scrollPosition;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, style, PackOptions(width, height));
            return !lastPosition.Equals(scrollPosition);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static void EndScrollView()
        {
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif
