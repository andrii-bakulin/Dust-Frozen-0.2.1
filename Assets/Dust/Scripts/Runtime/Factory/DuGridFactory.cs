using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Grid Factory")]
    [ExecuteInEditMode]
    public class DuGridFactory : DuFactory
    {
        public enum OffsetDirection
        {
            Disabled = 0,
            X = 1,
            Y = 2,
            Z = 3,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3Int m_Count = new Vector3Int(3, 1, 3);
        public Vector3Int count
        {
            get => m_Count;
            set
            {
                if (!UpdatePropertyValue(ref m_Count, Normalizer.Count(value)))
                    return;

                RebuildInstances();
            }
        }

        [SerializeField]
        private Vector3 m_Size = new Vector3(3f, 3f, 3f);
        public Vector3 size
        {
            get => m_Size;
            set
            {
                if (!UpdatePropertyValue(ref m_Size, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private OffsetDirection m_OffsetDirection = OffsetDirection.Disabled;
        public OffsetDirection offsetDirection
        {
            get => m_OffsetDirection;
            set
            {
                if (m_OffsetDirection == value)
                    return;

                m_OffsetDirection = value;
                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_OffsetWidth = 0f;
        public float offsetWidth
        {
            get => m_OffsetWidth;
            set
            {
                if (!UpdatePropertyValue(ref m_OffsetWidth, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_OffsetHeight = 0f;
        public float offsetHeight
        {
            get => m_OffsetHeight;
            set
            {
                if (!UpdatePropertyValue(ref m_OffsetHeight, value))
                    return;

                UpdateInstancesZeroStates();
            }
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
