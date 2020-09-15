using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Object Fields/Radial Field")]
    public class DuRadialField : DuObjectField
    {
        [SerializeField]
        private float m_StartAngle = 0.0f;
        public float startAngle
        {
            get => m_StartAngle;
            set => m_StartAngle = value;
        }

        [SerializeField]
        private float m_EndAngle = 360.0f;
        public float endAngle
        {
            get => m_EndAngle;
            set => m_EndAngle = value;
        }

        [SerializeField]
        private float m_FadeInOffset = 45.0f;
        public float fadeInOffset
        {
            get => m_FadeInOffset;
            set => m_FadeInOffset = ShapeNormalizer.FadeOffset(value);
        }

        [SerializeField]
        private float m_FadeOutOffset = 45.0f;
        public float fadeOutOffset
        {
            get => m_FadeOutOffset;
            set => m_FadeOutOffset = ShapeNormalizer.FadeOffset(value);
        }

        [SerializeField]
        private float m_Iterations = 1.0f;
        public float iterations
        {
            get => m_Iterations;
            set => m_Iterations = ShapeNormalizer.Iterations(value);
        }

        [SerializeField]
        private float m_Offset = 0.0f;
        public float offset
        {
            get => m_Offset;
            set => m_Offset = value;
        }

        [SerializeField]
        private Axis3xDirection m_Direction = Axis3xDirection.Y;
        public Axis3xDirection direction
        {
            get => m_Direction;
            set => m_Direction = value;
        }

        [SerializeField]
        private float m_GizmoLength = 2.0f;
        public float gizmoLength
        {
            get => m_GizmoLength;
            set => m_GizmoLength = ShapeNormalizer.GizmoLength(value);
        }

        [SerializeField]
        private float m_GizmoRadius = 1.0f;
        public float gizmoRadius
        {
            get => m_GizmoRadius;
            set => m_GizmoRadius = ShapeNormalizer.GizmoRadius(value);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Object Fields/Radial")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuRadialField));
        }
#endif

        public override string FieldName()
        {
            return "Radial";
        }

        //--------------------------------------------------------------------------------------------------------------
        //
        //               *--------------*                    A. Start Angle
        //             /                 \                   B. Start Angle + Start Transition
        //           /                    \                  C. End Angle - End Transition
        //         /                       \                 D. End Angle
        // *-----*                          *------*
        // 0'    A       B              C   D      360'
        //
        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            // Convert to [X+]-axis-space by direction
            localPosition = DuAxisDirection.ConvertFromDirectionToAxisXPlus(direction, localPosition);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            float angle = Vector2.SignedAngle(Vector2.down, new Vector2(localPosition.y, localPosition.z));
            angle = DuMath.NormalizeAngle360(angle - offset);        // step 1
            angle = DuMath.NormalizeAngle360(angle * iterations);    // step 2

            float power = 0f;

            if (DuMath.Between(angle, startAngle, endAngle))
            {
                float pointA = startAngle;
                float pointB = startAngle + fadeInOffset;
                float pointC = endAngle - fadeOutOffset;
                float pointD = endAngle;

                if (DuMath.Between(angle, pointB, pointC))
                {
                    power = 1f;
                }
                else // somewhere in transition(s). But may be in both transitions so get min value
                {
                    bool inTransitionAB = DuMath.Between(angle, pointA, pointB);
                    bool inTransitionCD = DuMath.Between(angle, pointC, pointD);

                    if (inTransitionAB && inTransitionCD)
                    {
                        power = Mathf.Min(Mathf.InverseLerp(pointA, pointB, angle), 1f - Mathf.InverseLerp(pointC, pointD, angle));
                    }
                    else if (inTransitionAB)
                    {
                        power = Mathf.InverseLerp(pointA, pointB, angle);
                    }
                    else if (inTransitionCD)
                    {
                        power = 1f - Mathf.InverseLerp(pointC, pointD, angle);
                    }
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            return remapping.MapValue(power, timeSinceStart);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        protected override void DrawFieldGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = !remapping.invert ? k_GizmosColorRangeOne : k_GizmosColorRangeZero;
            DrawRadialGizmo(startAngle + fadeInOffset, endAngle - fadeOutOffset, 0.95f);

            Gizmos.color = !remapping.invert ? k_GizmosColorRangeZero : k_GizmosColorRangeOne;
            DrawRadialGizmo(startAngle, endAngle, 1f);
        }

        protected void DrawRadialGizmo(float angleStart, float angleEnd, float scale)
        {
            Vector3 halfLengthOffset = new Vector3(gizmoLength / 2f, 0f, 0f);
            halfLengthOffset = DuAxisDirection.ConvertFromAxisXPlusToDirection(direction, halfLengthOffset);

            int points = 64;

            float angleDelta = (angleEnd - angleStart) / points;

            for (int i = 0; i < points; i++)
            {
                float angle0 = angleStart + i * angleDelta;
                float angle1 = angleStart + (i + 1) * angleDelta;

                Vector3 p0 = DuGizmos.GetCirclePointByAngle(180f - angle0, direction) * gizmoRadius * scale;
                Vector3 p1 = DuGizmos.GetCirclePointByAngle(180f - angle1, direction) * gizmoRadius * scale;

                Gizmos.DrawLine(p0 + halfLengthOffset, p1 + halfLengthOffset);
                Gizmos.DrawLine(p0 - halfLengthOffset, p1 - halfLengthOffset);
            }

            Vector3 pMid = Vector3.zero;
            Vector3 pBeg = DuGizmos.GetCirclePointByAngle(180f - angleStart, direction) * gizmoRadius * scale;
            Vector3 pEnd = DuGizmos.GetCirclePointByAngle(180f - angleEnd, direction) * gizmoRadius * scale;

            Gizmos.DrawLine(pMid - halfLengthOffset, pMid + halfLengthOffset);
            Gizmos.DrawLine(pBeg - halfLengthOffset, pBeg + halfLengthOffset);
            Gizmos.DrawLine(pEnd - halfLengthOffset, pEnd + halfLengthOffset);

            Gizmos.DrawLine(pMid - halfLengthOffset, pBeg - halfLengthOffset);
            Gizmos.DrawLine(pMid - halfLengthOffset, pEnd - halfLengthOffset);

            Gizmos.DrawLine(pMid + halfLengthOffset, pBeg + halfLengthOffset);
            Gizmos.DrawLine(pMid + halfLengthOffset, pEnd + halfLengthOffset);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        internal static class ShapeNormalizer
        {
            public static float FadeOffset(float value)
            {
                return Mathf.Clamp(value, 0f, 360f);
            }

            public static float Iterations(float value)
            {
                return Mathf.Max(1f, value);
            }

            public static float GizmoLength(float value)
            {
                return Mathf.Abs(value);
            }

            public static float GizmoRadius(float value)
            {
                return Mathf.Abs(value);
            }
        }
    }
}
