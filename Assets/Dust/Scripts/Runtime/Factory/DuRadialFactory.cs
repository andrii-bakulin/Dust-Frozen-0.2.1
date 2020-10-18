﻿using UnityEngine;

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
            set
            {
                if (!UpdatePropertyValue(ref m_Count, Normalizer.Count(value)))
                    return;

                RebuildInstances();
            }
        }

        [SerializeField]
        private float m_Radius = 1.0f;
        public float radius
        {
            get => m_Radius;
            set
            {
                if (!UpdatePropertyValue(ref m_Radius, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private Orientation m_Orientation = Orientation.XZ;
        public Orientation orientation
        {
            get => m_Orientation;
            set
            {
                if (m_Orientation == value)
                    return;

                m_Orientation = value;
                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private bool m_Align = true;
        public bool align
        {
            get => m_Align;
            set
            {
                if (!UpdatePropertyValue(ref m_Align, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_StartAngle = 0f;
        public float startAngle
        {
            get => m_StartAngle;
            set
            {
                if (!UpdatePropertyValue(ref m_StartAngle, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_EndAngle = 360f;
        public float endAngle
        {
            get => m_EndAngle;
            set
            {
                if (!UpdatePropertyValue(ref m_EndAngle, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_Offset = 0f;
        public float offset
        {
            get => m_Offset;
            set
            {
                if (!UpdatePropertyValue(ref m_Offset, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private float m_OffsetVariation = 0f;
        public float offsetVariation
        {
            get => m_OffsetVariation;
            set
            {
                if (!UpdatePropertyValue(ref m_OffsetVariation, value))
                    return;

                UpdateInstancesZeroStates();
            }
        }

        [SerializeField]
        private int m_OffsetSeed = DuConstants.RANDOM_SEED_DEFAULT;
        public int offsetSeed
        {
            get => m_OffsetSeed;
            set
            {
                if (!UpdatePropertyValue(ref m_OffsetSeed, Normalizer.OffsetSeed(value)))
                    return;

                UpdateInstancesZeroStates();
            }
        }

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
