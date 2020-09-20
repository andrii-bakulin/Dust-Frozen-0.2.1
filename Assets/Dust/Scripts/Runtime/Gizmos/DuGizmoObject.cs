using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    public abstract class DuGizmoObject : DuMonoBehaviour
    {
        protected static readonly Color k_GizmosDefaultColor = new Color(1.00f, 0.66f, 0.33f);

        public enum GizmosVisibility
        {
            DrawOnSelect = 1,
            AlwaysDraw = 2,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Color m_Color = k_GizmosDefaultColor;
        public Color color
        {
            get => m_Color;
            set => m_Color = value;
        }

        [SerializeField]
        private GizmosVisibility m_GizmoVisibility = GizmosVisibility.AlwaysDraw;

        public GizmosVisibility gizmoVisibility
        {
            get => m_GizmoVisibility;
            set => m_GizmoVisibility = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void AddGizmoToSelectedOrNewObject(System.Type duComponentType)
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
                AddGizmoToNewObject(duComponentType);
            }
        }

        public static Component AddGizmoToNewObject(System.Type duComponentType, bool fixUndoState = true)
        {
            var gameObject = new GameObject();

            if (Dust.IsNotNull(Selection.activeGameObject))
                gameObject.transform.parent = Selection.activeGameObject.transform;

            var component = gameObject.AddComponent(duComponentType) as DuGizmoObject;

            gameObject.name = component.GizmoName() + " Gizmo";
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;

            if (fixUndoState)
                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            Selection.activeGameObject = gameObject;
            return component;
        }

        //--------------------------------------------------------------------------------------------------------------

        public abstract string GizmoName();

        void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmoVisibility != GizmosVisibility.AlwaysDraw)
                return;

            DrawGizmos();
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmos();
        }

        protected abstract void DrawGizmos();
    }
}
#endif
