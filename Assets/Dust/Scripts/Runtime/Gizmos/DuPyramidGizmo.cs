﻿using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Pyramid Gizmo")]
    public class DuPyramidGizmo : DuGizmoObject
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
        private int m_Faces = 4;
        public int faces
        {
            get => m_Faces;
            set => m_Faces = Normalizer.Faces(value);
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

        [MenuItem("Dust/Gizmos/Pyramid")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Pyramid Gizmo", typeof(DuPyramidGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void DrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = color;

            DuGizmos.DrawWirePyramid(radius, height, center, direction, faces);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static int Faces(int value)
            {
                return Mathf.Max(3, value);
            }
        }
    }
}
#endif