using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Cube Gizmo")]
    public class DuCubeGizmo : DuGizmoObject
    {
        [SerializeField]
        private Vector3 m_Size = Vector3.one;
        public Vector3 size
        {
            get => m_Size;
            set => m_Size = value;
        }

        [SerializeField]
        private Vector3 m_Center = Vector3.zero;
        public Vector3 center
        {
            get => m_Center;
            set => m_Center = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Cube")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(DuCubeGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string GizmoName()
        {
            return "Cube";
        }

        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = color;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
#endif
