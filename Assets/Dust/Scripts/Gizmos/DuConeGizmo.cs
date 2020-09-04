using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Cone Gizmo")]
    public class DuConeGizmo : DuGizmo
    {
        [SerializeField]
        private float m_Radius = 1f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = value;
        }

        [SerializeField]
        private float m_Height = 2f;
        public float height
        {
            get => m_Height;
            set => m_Height = value;
        }

        [SerializeField]
        private Vector3 m_Center = Vector3.zero;
        public Vector3 center
        {
            get => m_Center;
            set => m_Center = value;
        }

        [SerializeField]
        private Axis6xDirection m_Direction = Axis6xDirection.YPlus;
        public Axis6xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Cone")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Cone Gizmo", typeof(DuConeGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = color;

            DrawWireCone(radius, height, center, direction, 64, 4);
        }
    }
}
#endif
