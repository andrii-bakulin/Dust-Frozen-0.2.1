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
        private bool m_WeightVisible = false;
        public bool weightVisible
        {
            get => m_WeightVisible;
            set => m_WeightVisible = value;
        }

        [SerializeField]
        private float m_WeightSize = 1f;
        public float weightSize
        {
            get => m_WeightSize;
            set => m_WeightSize = Normalizer.Size(value);
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
            AddComponentToSelectedOrNewObject("Fields Space Gizmo", typeof(DuFieldsSpaceGizmo));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void DrawGizmos()
        {
            if (Dust.IsNull(fieldsSpace))
                return;

            if (!weightVisible && !colorVisible)
                return;

            GUIStyle style = new GUIStyle("Label");
            style.fontSize = Mathf.RoundToInt(style.fontSize * weightSize);

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
                float fieldWeight = fieldsSpace.GetWeightAndColor(worldPosition, out fieldColor);

                if (colorVisible)
                {
                    if (!colorAllowTransparent)
                        fieldColor = DuColorBlend.AlphaBlend(Color.black, fieldColor);

                    Handles.color = fieldColor;
                    Handles.DotHandleCap(0, worldPosition, transform.rotation, 0.1f * colorSize, EventType.Repaint);
                }

                if (weightVisible)
                {
                    Handles.Label(worldPosition, fieldWeight.ToString("F2"), style);
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
