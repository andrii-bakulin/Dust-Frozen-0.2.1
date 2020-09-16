using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        public static void AddComponent(System.Type duComponentType)
        {
            if (Selection.gameObjects.Length == 0)
                return;

            foreach (var gameObject in Selection.gameObjects)
            {
                Undo.AddComponent(gameObject, duComponentType);
            }
        }

        public static void AddComponentToSelectedOrNewObject(string gameObjectName, System.Type duComponentType)
        {
            if (Selection.gameObjects.Length > 0)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    Undo.AddComponent(gameObject, duComponentType);
                }
            }
            else
            {
                AddComponentToNewObject(gameObjectName, duComponentType);
            }
        }

        public static Component AddComponentToNewObject(string gameObjectName, System.Type duComponentType, bool fixUndoState = true)
        {
            var gameObject = new GameObject();
            gameObject.name = gameObjectName;

            if (Dust.IsNotNull(Selection.activeGameObject))
                gameObject.transform.parent = Selection.activeGameObject.transform;

            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;

            var component = gameObject.AddComponent(duComponentType);

            if (fixUndoState)
                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            Selection.activeGameObject = gameObject;
            return component;
        }

        protected static bool IsAllowExecMenuCommandOnce(MenuCommand menuCommand)
        {
            return Selection.objects.Length == 0 || menuCommand.context == Selection.objects[0];
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        internal Space WorkSpaceToSpace(WorkSpace space)
        {
            if (space == WorkSpace.World)
                return Space.World;

            // WorkSpace.Local [or] default
            return Space.Self;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        private float editorUpdate_deltaTime;
        private double editorUpdate_lastTimeStamp;

        protected void EditorUpdateReset()
        {
            editorUpdate_deltaTime = 0f;
            editorUpdate_lastTimeStamp = -1;
        }

        protected bool EditorUpdateTick(out float deltaTime)
        {
            deltaTime = 0f;

            if (editorUpdate_lastTimeStamp <= 0)
            {
                editorUpdate_lastTimeStamp = EditorApplication.timeSinceStartup;
                return false;
            }

            editorUpdate_deltaTime += (float) (EditorApplication.timeSinceStartup - editorUpdate_lastTimeStamp);
            editorUpdate_lastTimeStamp = EditorApplication.timeSinceStartup;

            if (editorUpdate_deltaTime < DuConstants.EDITOR_UPDATE_TIMEOUT)
                return false;

            deltaTime = editorUpdate_deltaTime;
            editorUpdate_deltaTime = 0f;

            return true;
        }
#endif
    }
}
