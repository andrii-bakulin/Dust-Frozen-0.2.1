using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Mesh Gizmo")]
    public class DuMeshGizmo : DuGizmo
    {
        [SerializeField]
        private Mesh m_Mesh;
        public Mesh mesh
        {
            get => m_Mesh;
            set => m_Mesh = value;
        }

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;
        public Vector3 position
        {
            get => m_Position;
            set => m_Position = value;
        }

        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;
        public Vector3 rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        public Vector3 scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Mesh")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Mesh Gizmo", typeof(DuMeshGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = color;
            Gizmos.DrawWireMesh(mesh, position, Quaternion.Euler(rotation), scale);
        }
    }
}
#endif
