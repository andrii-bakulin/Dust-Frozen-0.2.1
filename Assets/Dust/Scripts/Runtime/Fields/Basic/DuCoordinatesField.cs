using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Basic Fields/Coordinates Field")]
    public class DuCoordinatesField : DuField
    {
        public enum ShapeMode
        {
            Linear = 0,
            Repeat = 1,
            PingPong = 2,
        }

        public enum AggregateMode
        {
            Avg = 0,
            Min = 1,
            Max = 2,
            Sum = 3,
            Sub = 4,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_PowerEnabled = true;
        public bool powerEnabled
        {
            get => m_PowerEnabled;
            set => m_PowerEnabled = value;
        }

        [SerializeField]
        private bool m_PowerUseAxisX = true;
        public bool powerUseAxisX
        {
            get => m_PowerUseAxisX;
            set => m_PowerUseAxisX = value;
        }

        [SerializeField]
        private bool m_PowerUseAxisY = true;
        public bool powerUseAxisY
        {
            get => m_PowerUseAxisY;
            set => m_PowerUseAxisY = value;
        }

        [SerializeField]
        private bool m_PowerUseAxisZ = true;
        public bool powerUseAxisZ
        {
            get => m_PowerUseAxisZ;
            set => m_PowerUseAxisZ = value;
        }

        [SerializeField]
        private float m_PowerScale = 1f;
        public float powerScale
        {
            get => m_PowerScale;
            set => m_PowerScale = value;
        }

        [SerializeField]
        private AggregateMode m_PowerAggregate = AggregateMode.Max;
        public AggregateMode powerAggregate
        {
            get => m_PowerAggregate;
            set => m_PowerAggregate = value;
        }

        [SerializeField]
        private float m_PowerMin = 0;
        public float powerMin
        {
            get => m_PowerMin;
            set => m_PowerMin = value;
        }

        [SerializeField]
        private float m_PowerMax = 1f;
        public float powerMax
        {
            get => m_PowerMax;
            set => m_PowerMax = value;
        }

        [SerializeField]
        private ShapeMode m_PowerShape = ShapeMode.PingPong;
        public ShapeMode powerShape
        {
            get => m_PowerShape;
            set => m_PowerShape = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ColorEnabled = true;
        public bool colorEnabled
        {
            get => m_ColorEnabled;
            set => m_ColorEnabled = value;
        }

        [SerializeField]
        private float m_ColorScale = 1f;
        public float colorScale
        {
            get => m_ColorScale;
            set => m_ColorScale = value;
        }

        [SerializeField]
        private ShapeMode m_ColorShape = ShapeMode.Linear;
        public ShapeMode colorShape
        {
            get => m_ColorShape;
            set => m_ColorShape = value;
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Basic Fields/Coordinates")]
        public static void AddComponent()
        {
            AddFieldComponentByType(typeof(DuCoordinatesField));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            int seq = 0, dynamicState = 0;

            DuDynamicState.Append(ref dynamicState, ++seq, transform);
            DuDynamicState.Append(ref dynamicState, ++seq, powerEnabled);

            if (powerEnabled)
            {
                DuDynamicState.Append(ref dynamicState, ++seq, powerUseAxisX);
                DuDynamicState.Append(ref dynamicState, ++seq, powerUseAxisY);
                DuDynamicState.Append(ref dynamicState, ++seq, powerUseAxisZ);
                DuDynamicState.Append(ref dynamicState, ++seq, powerScale);
                DuDynamicState.Append(ref dynamicState, ++seq, powerAggregate);
                DuDynamicState.Append(ref dynamicState, ++seq, powerMin);
                DuDynamicState.Append(ref dynamicState, ++seq, powerMax);
                DuDynamicState.Append(ref dynamicState, ++seq, powerShape);
            }

            DuDynamicState.Append(ref dynamicState, ++seq, colorEnabled);

            if (colorEnabled)
            {
                DuDynamicState.Append(ref dynamicState, ++seq, colorScale);
                DuDynamicState.Append(ref dynamicState, ++seq, colorShape);
            }

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FieldName()
        {
            return "Coordinates";
        }

        public override float GetPowerForFieldPoint(DuField.Point fieldPoint)
        {
            if (!powerEnabled)
                return 0f;

            Vector3 point = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition);

            point = DuVector3.Abs(point);

            float value = 0f;

            if (powerUseAxisX && powerUseAxisY && powerUseAxisZ)
            {
                switch (powerAggregate)
                {
                    default:
                    case AggregateMode.Avg: value = (point.x + point.y + point.z) / 3f; break;
                    case AggregateMode.Min: value = Mathf.Min(Mathf.Min(point.x, point.y), point.z); break;
                    case AggregateMode.Max: value = Mathf.Max(Mathf.Max(point.x, point.y), point.z); break;
                    case AggregateMode.Sum: value = +(point.x + point.y + point.z); break;
                    case AggregateMode.Sub: value = -(point.x + point.y + point.z); break;
                }
            }
            else if (powerUseAxisX && powerUseAxisY)
            {
                switch (powerAggregate)
                {
                    default:
                    case AggregateMode.Avg: value = (point.x + point.y) / 2f; break;
                    case AggregateMode.Min: value = Mathf.Min(point.x, point.y); break;
                    case AggregateMode.Max: value = Mathf.Max(point.x, point.y); break;
                    case AggregateMode.Sum: value = +(point.x + point.y); break;
                    case AggregateMode.Sub: value = -(point.x + point.y); break;
                }
            }
            else if (powerUseAxisY && powerUseAxisZ)
            {
                switch (powerAggregate)
                {
                    default:
                    case AggregateMode.Avg: value = (point.y + point.z) / 2f; break;
                    case AggregateMode.Min: value = Mathf.Min(point.y, point.z); break;
                    case AggregateMode.Max: value = Mathf.Max(point.y, point.z); break;
                    case AggregateMode.Sum: value = +(point.y + point.z); break;
                    case AggregateMode.Sub: value = -(point.y + point.z); break;
                }
            }
            else if (powerUseAxisX && powerUseAxisZ)
            {
                switch (powerAggregate)
                {
                    default:
                    case AggregateMode.Avg: value = (point.x + point.z) / 2f; break;
                    case AggregateMode.Min: value = Mathf.Min(point.x, point.z); break;
                    case AggregateMode.Max: value = Mathf.Max(point.x, point.z); break;
                    case AggregateMode.Sum: value = +(point.x + point.z); break;
                    case AggregateMode.Sub: value = -(point.x + point.z); break;
                }
            }
            else if (powerUseAxisX)
            {
                value = point.x;
            }
            else if (powerUseAxisY)
            {
                value = point.y;
            }
            else if (powerUseAxisZ)
            {
                value = point.z;
            }

            return RepackValueByShape(powerShape, value * powerScale, powerMin, powerMax);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool IsAllowGetFieldColor()
        {
            return colorEnabled;
        }

        public override Color GetFieldColor(DuField.Point fieldPoint, float powerByField)
        {
            if (DuMath.IsZero(colorScale))
                return Color.black;

            Color color = transform.worldToLocalMatrix.MultiplyPoint(fieldPoint.inPosition).ToColor();

            color.r = RepackValueByShape(colorShape, color.r / colorScale, 0f, 1f);
            color.g = RepackValueByShape(colorShape, color.g / colorScale, 0f, 1f);
            color.b = RepackValueByShape(colorShape, color.b / colorScale, 0f, 1f);

            return color;
        }

        //--------------------------------------------------------------------------------------------------------------

        public float RepackValueByShape(ShapeMode shape, float value, float min, float max)
        {
            switch (shape)
            {
                default:
                case ShapeMode.Linear:
                    return value;

                case ShapeMode.Repeat:
                    if (min.Equals(max))
                        return 0f;

                    return min + Mathf.Repeat(value, max - min);

                case ShapeMode.PingPong:
                    if (min.Equals(max))
                        return 0f;

                    return min + Mathf.PingPong(value, max - min);
            }
        }
    }
}
