using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Linear Factory")]
    [ExecuteInEditMode]
    public class DuLinearFactory : DuFactory
    {
        [SerializeField]
        private int m_Count = 5;
        public int count
        {
            get => m_Count;
            set => m_Count = Normalizer.Count(value);
        }

        [SerializeField]
        private int m_Offset = 0;
        public int offset
        {
            get => m_Offset;
            set => m_Offset = Normalizer.Offset(value);
        }

        [SerializeField]
        private float m_Amount = 1f;
        public float amount
        {
            get => m_Amount;
            set => m_Amount = value;
        }

        [SerializeField]
        private Vector3 m_Position = Vector3.right * 5f;
        public Vector3 position
        {
            get => m_Position;
            set => m_Position = value;
        }

        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;
        public Vector3 rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        public Vector3 scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        [SerializeField]
        private Vector3 m_StepRotation = Vector3.zero;
        public Vector3 stepRotation
        {
            get => m_StepRotation;
            set => m_StepRotation = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryName()
        {
            return "Linear";
        }

        internal override DuFactoryBuilder GetFactoryBuilder()
        {
            return new DuFactoryLinearBuilder();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public class Normalizer
        {
            public static int Count(int value)
            {
                return Mathf.Max(0, value);
            }

            public static int Offset(int value)
            {
                return Mathf.Max(0, value);
            }
        }
    }
}
