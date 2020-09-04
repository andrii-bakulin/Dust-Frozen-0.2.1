﻿using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    public abstract class DuGizmo : DuMonoBehaviour
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
        private GizmosVisibility m_GizmosVisibility = GizmosVisibility.AlwaysDraw;

        public GizmosVisibility gizmosVisibility
        {
            get => m_GizmosVisibility;
            set => m_GizmosVisibility = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // All Gizmos by default draw elements in [X+]-axis-direction

        public static void DrawWireCylinder(float radius, float height, Vector3 center, Axis3xDirection direction, int circlePoints, int edgesCount)
        {
            DrawWireCylinder(radius, height, center, DuAxisDirection.ConvertToAxis6(direction), circlePoints, edgesCount);
        }

        public static void DrawWireCylinder(float radius, float height, Vector3 center, Axis6xDirection direction, int circlePoints, int edgesCount)
        {
            Vector3 pHeight = new Vector3(height / 2f, 0f, 0f);
            pHeight = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pHeight);

            for (int i = 0; i < circlePoints; i++)
            {
                float offset0 = (float) i / circlePoints;
                float offset1 = (float) (i + 1) / circlePoints;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction) * radius;
                Vector3 p1 = GetCirclePointByOffset(offset1, direction) * radius;

                Gizmos.DrawLine(center + p0 + pHeight, center + p1 + pHeight);
                Gizmos.DrawLine(center + p0 - pHeight, center + p1 - pHeight);
            }

            for (int i = 0; i < edgesCount; i++)
            {
                float offset0 = (float) i / edgesCount;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction) * radius;

                Gizmos.DrawLine(center + p0 + pHeight, center + p0 - pHeight);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void DrawWireCone(float radius, float height, Vector3 center, Axis6xDirection direction, int circlePoints, int edgesCount)
        {
            Vector3 pHeight = new Vector3(height / 2f, 0f, 0f);
            pHeight = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, pHeight);

            for (int i = 0; i < circlePoints; i++)
            {
                float offset0 = (float) i / circlePoints;
                float offset1 = (float) (i + 1) / circlePoints;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction);
                Vector3 p1 = GetCirclePointByOffset(offset1, direction);

                // Middle
                Gizmos.DrawLine(center + p0 * radius / 2f, center + p1 * radius / 2f);

                // Base
                Gizmos.DrawLine(center + p0 * radius - pHeight, center + p1 * radius - pHeight);
            }

            for (int i = 0; i < edgesCount; i++)
            {
                float offset0 = (float) i / edgesCount;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction) * radius;

                Gizmos.DrawLine(center + pHeight, center + p0 - pHeight);

                if (i < edgesCount / 2)
                {
                    // Lines in base, but only for "half of count"
                    Gizmos.DrawLine(center + p0 - pHeight, center - p0 - pHeight);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void DrawWireTorus(float radius, float thickness, Vector3 center, Axis6xDirection direction, int circlePoints, int smallCirclePoints)
        {
            DrawWireTorus(radius, thickness, center, DuAxisDirection.ConvertToAxis3(direction), circlePoints, smallCirclePoints);
        }

        public static void DrawWireTorus(float radius, float thickness, Vector3 center, Axis3xDirection direction, int circlePoints, int smallCirclePoints)
        {
            Vector3 offset0;
            Vector3 offsetA;
            Vector3 offsetB;
            Axis3xDirection directionA;
            Axis3xDirection directionB;

            switch (direction)
            {
                default:
                case Axis3xDirection.X:
                    offset0 = new Vector3(1f, 0f, 0f);

                    offsetA = new Vector3(0f, 0f, 1f);
                    offsetB = new Vector3(0f, 1f, 0f);
                    directionA = Axis3xDirection.Y;
                    directionB = Axis3xDirection.Z;
                    break;

                case Axis3xDirection.Y:
                    offset0 = new Vector3(0f, 1f, 0f);

                    offsetA = new Vector3(0f, 0f, 1f);
                    offsetB = new Vector3(1f, 0f, 0f);
                    directionA = Axis3xDirection.X;
                    directionB = Axis3xDirection.Z;
                    break;

                case Axis3xDirection.Z:
                    offset0 = new Vector3(0f, 0f, 1f);

                    offsetA = new Vector3(0f, 1f, 0f);
                    offsetB = new Vector3(1f, 0f, 0f);
                    directionA = Axis3xDirection.X;
                    directionB = Axis3xDirection.Y;
                    break;
            }

            DrawCircle(radius + thickness, center, direction, 64);
            DrawCircle(radius - thickness, center, direction, 64);
            DrawCircle(radius, center + (offset0 * +thickness), direction, 64);
            DrawCircle(radius, center + (offset0 * -thickness), direction, 64);

            DrawCircle(thickness, center + radius * offsetA, directionA, 32);
            DrawCircle(thickness, center - radius * offsetA, directionA, 32);

            DrawCircle(thickness, center + radius * offsetB, directionB, 32);
            DrawCircle(thickness, center - radius * offsetB, directionB, 32);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void DrawCircle(float radius, Vector3 center, Axis3xDirection direction, int circlePoints)
        {
            DrawCircle(radius, center, DuAxisDirection.ConvertToAxis6(direction), circlePoints);
        }

        public static void DrawCircle(float radius, Vector3 center, Axis6xDirection direction, int circlePoints)
        {
            for (int i = 0; i < circlePoints; i++)
            {
                float offset0 = (float) i / circlePoints;
                float offset1 = (float) (i + 1) / circlePoints;

                Vector3 p0 = GetCirclePointByOffset(offset0, direction) * radius;
                Vector3 p1 = GetCirclePointByOffset(offset1, direction) * radius;

                Gizmos.DrawLine(center + p0, center + p1);
                Gizmos.DrawLine(center + p0, center + p1);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 GetCirclePointByAngle(float angle, Axis3xDirection direction = Axis3xDirection.X)
        {
            return GetCirclePointByOffset(angle / 360f, DuAxisDirection.ConvertToAxis6(direction));
        }

        public static Vector3 GetCirclePointByOffset(float offset, Axis3xDirection direction = Axis3xDirection.X)
        {
            return GetCirclePointByOffset(offset, DuAxisDirection.ConvertToAxis6(direction));
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Vector3 GetCirclePointByAngle(float angle, Axis6xDirection direction = Axis6xDirection.XPlus)
        {
            return GetCirclePointByOffset(angle / 360f, direction);
        }

        public static Vector3 GetCirclePointByOffset(float offset, Axis6xDirection direction = Axis6xDirection.XPlus)
        {
            Vector3 point = new Vector3(0f, Mathf.Sin(DuConstants.PI2 * offset), Mathf.Cos(DuConstants.PI2 * offset));

            return DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, point);
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnDrawGizmos()
        {
            if (Selection.activeGameObject == this.gameObject)
                return;

            if (gizmosVisibility != GizmosVisibility.AlwaysDraw)
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
