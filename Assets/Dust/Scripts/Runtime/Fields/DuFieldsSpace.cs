using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Fields/Fields Space")]
    public class DuFieldsSpace : DuMonoBehaviour
    {
        [SerializeField]
        private DuFieldsMap m_FieldsMap = DuFieldsMap.FieldsSpace();
        public DuFieldsMap fieldsMap => m_FieldsMap;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuField.Point m_CalcFieldPoint = new DuField.Point();

        //--------------------------------------------------------------------------------------------------------------

        public float GetPower(Vector3 worldPosition)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0;

            fieldsMap.Calculate(m_CalcFieldPoint);

            return m_CalcFieldPoint.outPower;
        }

        public Color GetColor(Vector3 worldPosition)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0;

            fieldsMap.Calculate(m_CalcFieldPoint);

            return m_CalcFieldPoint.outColor;
        }

        public float GetPowerAndColor(Vector3 worldPosition, out Color color)
        {
            m_CalcFieldPoint.inPosition = worldPosition;
            m_CalcFieldPoint.inOffset = 0;

            fieldsMap.Calculate(m_CalcFieldPoint);

            color = m_CalcFieldPoint.outColor;
            return m_CalcFieldPoint.outPower;
        }
    }
}
