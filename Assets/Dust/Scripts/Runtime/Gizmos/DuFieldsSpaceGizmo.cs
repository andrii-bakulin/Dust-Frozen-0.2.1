using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine
{
    [AddComponentMenu("Dust/Gizmos/Fields Space Gizmo")]
    [ExecuteInEditMode]
    public class DuFieldsSpaceGizmo : DuGizmoObject
    {
        [SerializeField]
        private DuFieldsSpace m_FieldsSpace;
        public DuFieldsSpace fieldsSpace
        {
            get => m_FieldsSpace;
            set => m_FieldsSpace = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Vector3Int m_GridCount = new Vector3Int(9, 1, 9);
        public Vector3Int gridCount
        {
            get => m_GridCount;
            set => m_GridCount = Normalizer.GridCount(value);
        }

        [SerializeField]
        private Vector3 m_GridStep = Vector3.one;
        public Vector3 gridStep
        {
            get => m_GridStep;
            set => m_GridStep = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_PowerVisible = false;
        public bool powerVisible
        {
            get => m_PowerVisible;
            set => m_PowerVisible = value;
        }

        [SerializeField]
        private float m_PowerSize = 1f;
        public float powerSize
        {
            get => m_PowerSize;
            set => m_PowerSize = Normalizer.Size(value);
        }

        [SerializeField]
        private bool m_PowerDotsVisible = false;
        public bool powerDotsVisible
        {
            get => m_PowerDotsVisible;
            set => m_PowerDotsVisible = value;
        }

        [SerializeField]
        private float m_PowerDotsSize = 0.4f;
        public float powerDotsSize
        {
            get => m_PowerDotsSize;
            set => m_PowerDotsSize = Normalizer.Size(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private bool m_ColorVisible = true;
        public bool colorVisible
        {
            get => m_ColorVisible;
            set => m_ColorVisible = value;
        }

        [SerializeField]
        private float m_ColorSize = 1f;
        public float colorSize
        {
            get => m_ColorSize;
            set => m_ColorSize = Normalizer.Size(value);
        }

        [SerializeField]
        private bool m_ColorAllowTransparent = true;
        public bool colorAllowTransparent
        {
            get => m_ColorAllowTransparent;
            set => m_ColorAllowTransparent = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Gizmos/Fields Space")]
        public static void AddComponentToSelectedObjects()
        {
            AddGizmoToSelectedOrNewObject(typeof(DuFieldsSpaceGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string GizmoName()
        {
            return "Fields Space";
        }

        protected override void DrawGizmos()
        {
            if (Dust.IsNull(fieldsSpace))
                return;

            bool showPower = powerVisible && fieldsSpace.fieldsMap.calculatePower;
            bool showColor = colorVisible && fieldsSpace.fieldsMap.calculateColor;

            if (!showPower && !showColor)
                return;

            GUIStyle style = new GUIStyle("Label");
            style.fontSize = Mathf.RoundToInt(style.fontSize * powerSize);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            Vector3 zeroPoint;
            zeroPoint.x = -(gridCount.x - 1) / 2f * gridStep.x;
            zeroPoint.y = -(gridCount.y - 1) / 2f * gridStep.y;
            zeroPoint.z = -(gridCount.z - 1) / 2f * gridStep.z;

            for (int z = 0; z < gridCount.z; z++)
            for (int y = 0; y < gridCount.y; y++)
            for (int x = 0; x < gridCount.x; x++)
            {
                var position = zeroPoint + new Vector3(gridStep.x * x, gridStep.y * y, gridStep.z * z);

                Vector3 worldPosition = transform.TransformPoint(position);

                Color fieldColor;
                float fieldPower = fieldsSpace.GetPowerAndColor(worldPosition, out fieldColor);

                if (showColor)
                {
                    if (!colorAllowTransparent)
                        fieldColor = DuColorBlend.AlphaBlend(Color.black, fieldColor);

                    Handles.color = fieldColor;
                    Handles.DotHandleCap(0, worldPosition, transform.rotation, 0.1f * colorSize, EventType.Repaint);
                }

                if (showPower)
                {
                    Handles.Label(worldPosition, fieldPower.ToString("F2"), style);

                    if (powerDotsVisible)
                    {
                        Handles.color = Color.Lerp(Color.black, DuObjectField.k_GizmosColorRangeOne, fieldPower);
                        Handles.DotHandleCap(0, worldPosition, transform.rotation, 0.1f * powerDotsSize, EventType.Repaint);
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public class Normalizer
        {
            public static float Size(float value)
            {
                return Mathf.Clamp(value, 0.1f, float.MaxValue);
            }

            public static Vector3Int GridCount(Vector3Int value)
            {
                return DuVector3Int.Clamp(value, Vector3Int.one, Vector3Int.one * 1000);
            }
        }
    }
}
#endif
