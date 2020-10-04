using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Grid Factory")]
    [ExecuteInEditMode]
    public class DuGridFactory : DuFactory
    {
        [SerializeField]
        private Vector3Int m_Count = new Vector3Int(3, 1, 3);
        public Vector3Int count
        {
            get => m_Count;
            set => m_Count = Normalizer.Count(value);
        }

        [SerializeField]
        private Vector3 m_Size = new Vector3(3f, 3f, 3f);
        public Vector3 size
        {
            get => m_Size;
            set => m_Size = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryName()
        {
            return "Grid";
        }

        internal override DuFactoryBuilder GetFactoryBuilder()
        {
            return new DuFactoryGridBuilder();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public class Normalizer
        {
            public static Vector3Int Count(Vector3Int value)
            {
                return DuVector3Int.Clamp(value, Vector3Int.one, Vector3Int.one * 1000);
            }
        }
    }
}
