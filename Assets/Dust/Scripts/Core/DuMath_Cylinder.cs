using UnityEngine;

namespace DustEngine
{
    // Math for this cylinder use fixed orientation along [X+]-axis
    // Ray outgoing from center of cylinder to endPoint
    public static partial class DuMath
    {
        public static class Cylinder
        {
            public static Vector3 IntersectionPoint(float radius, float height, Vector3 endPoint)
            {
                if (IsZero(radius) || IsZero(height) || endPoint.Equals(Vector3.zero))
                    return Vector3.zero;

                float h2 = height / 2f;

                Vector3 point = Vector3.zero;

                if (IsZero(endPoint.y) && IsZero(endPoint.z))
                    return new Vector3(h2 * Mathf.Sign(endPoint.z), 0f, 0f);

                float rP2 = radius * radius;
                float yP2 = endPoint.y * endPoint.y;
                float zP2 = endPoint.z * endPoint.z;

                point.y = Mathf.Sqrt(rP2 * yP2 / (yP2 + zP2)) * Mathf.Sign(endPoint.y);
                point.z = Mathf.Sqrt(rP2 * zP2 / (yP2 + zP2)) * Mathf.Sign(endPoint.z);

                point.x = endPoint.x * (IsNotZero(endPoint.y) ? point.y / endPoint.y : point.z / endPoint.z);

                if (Mathf.Abs(point.x) > h2)
                    point *= h2 / Mathf.Abs(point.x);

                return point;
            }

            public static float DistanceToEdge(float radius, float height, Vector3 endPoint)
            {
                return IntersectionPoint(radius, height, endPoint).magnitude;
            }
        }
    }
}
