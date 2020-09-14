using UnityEditor;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Fields Space")]
    public class DuFieldsSpace : DuMonoBehaviour
    {
        [SerializeField]
        protected DuFieldsMap m_FieldsMap = DuFieldsMap.WeightsAndColorsFieldsMap();
        public DuFieldsMap fieldsMap => m_FieldsMap;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Fields/Fields Space")]
        protected static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedOrNewObject("Fields Space", typeof(DuFieldsSpace));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public float GetWeight(Vector3 worldPosition)
        {
            float weight;
            fieldsMap.Calculate(worldPosition, 0.0f, out weight);
            return weight;
        }

        public Color GetColor(Vector3 worldPosition)
        {
            float weight;
            Color color;
            fieldsMap.Calculate(worldPosition, 0.0f, out weight, out color);
            return color;
        }

        public float GetWeightAndColor(Vector3 worldPosition, out Color color)
        {
            float weight;
            fieldsMap.Calculate(worldPosition, 0.0f, out weight, out color);
            return weight;
        }
    }
}
