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

        // ..
    }
}
