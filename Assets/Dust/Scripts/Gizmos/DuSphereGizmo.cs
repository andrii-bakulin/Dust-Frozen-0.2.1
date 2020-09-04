using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Sphere Gizmo")]
    public class DuSphereGizmo : DuGizmo
    {
        [SerializeField]
        private float m_Radius = 1f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = value;
        }

        [SerializeField]
        private Vector3 m_Center = Vector3.zero;
        public Vector3 center
        {
            get => m_Center;
            set => m_Center = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Sphere")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Sphere Gizmo", typeof(DuSphereGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = color;
            Gizmos.DrawWireSphere(center, radius);
        }
    }
}
#endif
