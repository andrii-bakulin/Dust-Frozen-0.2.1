using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Radial Factory")]
    [ExecuteInEditMode]
    public class DuRadialFactory : DuFactory
    {
        [SerializeField]
        private int m_Count = 5;
        public int count
        {
            get => m_Count;
            set => m_Count = Normalizer.Count(value);
        }

        [SerializeField]
        private float m_Radius = 1.0f;
        public float radius
        {
            get => m_Radius;
            set => m_Radius = value;
        }

        [SerializeField]
        private Orientation m_Orientation = Orientation.XZ;
        public Orientation orientation
        {
            get => m_Orientation;
            set => m_Orientation = value;
        }

        [SerializeField]
        private bool m_Align = true;
        public bool align
        {
            get => m_Align;
            set => m_Align = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_StartAngle = 0f;
        public float startAngle
        {
            get => m_StartAngle;
            set => m_StartAngle = value;
        }

        [SerializeField]
        private float m_EndAngle = 360f;
        public float endAngle
        {
            get => m_EndAngle;
            set => m_EndAngle = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_Offset = 0f;
        public float offset
        {
            get => m_Offset;
            set => m_Offset = value;
        }

        [SerializeField]
        private float m_OffsetVariation = 0f;
        public float offsetVariation
        {
            get => m_OffsetVariation;
            set => m_OffsetVariation = value;
        }

        [SerializeField]
        private int m_OffsetSeed = DuConstants.RANDOM_SEED_DEFAULT;
        public int offsetSeed
        {
            get => m_OffsetSeed;
            set => m_OffsetSeed = Normalizer.OffsetSeed(value);
        }

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Factory/Radial Factory")]
        public static void AddComponent()
        {
            CreateFactoryByType(typeof(DuRadialFactory));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryName()
        {
            return "Radial";
        }

        internal override DuFactoryBuilder GetFactoryBuilder()
        {
            return new DuFactoryRadialBuilder();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public class Normalizer
        {
            public static int Count(int value)
            {
                return Mathf.Max(0, value);
            }

            public static int OffsetSeed(int value)
            {
                return Mathf.Clamp(value, 1, DuConstants.RANDOM_SEED_MAX);
            }
        }
    }
}
