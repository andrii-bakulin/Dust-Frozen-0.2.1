using UnityEngine;

namespace DustEngine
{
    // Math for this cone use fixed orientation along [X+]-axis
    // Ray outgoing from center of cone to endPoint
    public static partial class DuMath
    {
        public static class Cone
        {
            /*
            public static Vector3 IntersectionPoint(float radius, float height, Vector3 endPoint)
            {
            }
            */

            public static float DistanceToEdge(float radius, float height, Vector3 endPoint)
            {
                if (IsZero(radius) || IsZero(height) || endPoint.Equals(Vector3.zero))
                    return 0f;

                if (DuMath.IsZero(endPoint.y) && DuMath.IsZero(endPoint.z))
                    return height / 2f;

                // conePoint1 .. conePoint2 = edge
                // conePoint2 .. conePoint3 = base

                Vector2 conePoint1 = new Vector2(+height / 2f, 0f);
                Vector2 conePoint2 = new Vector2(-height / 2f, radius);

                Vector2 linePoint1 = Vector2.zero;

                // Convert 3D point to 2D (x&z; y) -> (x; y)
                Vector2 linePoint2 = new Vector2(endPoint.x, DuMath.Length(endPoint.y, endPoint.z));
                linePoint2.y = Mathf.Abs(linePoint2.y);

                linePoint2 *= 1000f;
                Vector2 intersectPoint;

                if (DuVector2.IsIntersecting(conePoint1, conePoint2, linePoint1, linePoint2, out intersectPoint) == false)
                {
                    Vector2 conePoint3 = new Vector2(-height / 2f, 0f);

                    if (DuVector2.IsIntersecting(conePoint2, conePoint3, linePoint1, linePoint2, out intersectPoint) == false)
                        return 0f;
                }

                return intersectPoint.magnitude;
            }
        }
    }
}
