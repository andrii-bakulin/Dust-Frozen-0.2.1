﻿using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Noise Machine")]
    public class DuNoiseFactoryMachine : DuPRSFactoryMachine
    {
        public enum AxisRemapping
        {
            Off = 0,
            XyzToXzy = 1,
            XyzToYxz = 2,
            XyzToYzx = 3,
            XyzToZxy = 4,
            XyzToZyx = 5,
        }

        public enum NoiseDimension
        {
            Noise1D = 0,
            Noise3D = 1,
        }

        public enum NoiseMode
        {
            Random = 0,
            Perlin = 1,
        }

        public enum NoiseSpace
        {
            Global = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private NoiseMode m_NoiseMode = NoiseMode.Random;
        public NoiseMode noiseMode
        {
            get => m_NoiseMode;
            set => m_NoiseMode = value;
        }

        [SerializeField]
        private NoiseDimension m_NoiseDimension = NoiseDimension.Noise3D;
        public NoiseDimension noiseDimension
        {
            get => m_NoiseDimension;
            set => m_NoiseDimension = value;
        }

        [SerializeField]
        private float m_AnimationSpeed = 1f;
        public float animationSpeed
        {
            get => m_AnimationSpeed;
            set => m_AnimationSpeed = Normalizer.AnimationSpeed(value);
        }

        [SerializeField]
        private float m_AnimationOffset = 0f;
        public float animationOffset
        {
            get => m_AnimationOffset;
            set => m_AnimationOffset = value;
        }

        [SerializeField]
        private bool m_Synchronized = false;
        public bool synchronized
        {
            get => m_Synchronized;
            set => m_Synchronized = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        protected AxisRemapping m_PositionAxisRemapping = AxisRemapping.Off;
        public AxisRemapping positionAxisRemapping
        {
            get => m_PositionAxisRemapping;
            set => m_PositionAxisRemapping = value;
        }

        [SerializeField]
        protected AxisRemapping m_RotationAxisRemapping = AxisRemapping.Off;
        public AxisRemapping rotationAxisRemapping
        {
            get => m_RotationAxisRemapping;
            set => m_RotationAxisRemapping = value;
        }

        [SerializeField]
        protected AxisRemapping m_ScaleAxisRemapping = AxisRemapping.Off;
        public AxisRemapping scaleAxisRemapping
        {
            get => m_ScaleAxisRemapping;
            set => m_ScaleAxisRemapping = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private NoiseSpace m_NoiseSpace;
        public NoiseSpace noiseSpace
        {
            get => m_NoiseSpace;
            set => m_NoiseSpace = value;
        }

        [SerializeField]
        private float m_NoiseScale = 1f;
        public float noiseScale
        {
            get => m_NoiseScale;
            set => m_NoiseScale = Normalizer.NoiseScale(value);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed = DuConstants.RANDOM_SEED_DEFAULT;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (m_Seed == value)
                    return;

                m_Seed = value;
                ResetStates();
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuNoise m_DuNoisePos;
        private DuNoise m_DuNoiseRot;
        private DuNoise m_DuNoiseScl;

        private DuNoise duNoisePos => m_DuNoisePos ?? (m_DuNoisePos = new DuNoise(seed));
        private DuNoise duNoiseRot => m_DuNoiseRot ?? (m_DuNoiseRot = new DuNoise(seed + 1235));
        private DuNoise duNoiseScl => m_DuNoiseScl ?? (m_DuNoiseScl = new DuNoise(seed - 1235));

        private float m_OffsetDynamic;

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Noise";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_OffsetDynamic += Time.deltaTime * animationSpeed;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            Vector3 noisePowerPos;
            Vector3 noisePowerRot;
            Vector3 noisePowerScl;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            var factoryInstance = factoryInstanceState.instance;

            switch (noiseMode)
            {
                default:
                case NoiseMode.Random:
                {
                    var randomVector = factoryInstance.randomVector * 100f;

                    if (noiseDimension == NoiseDimension.Noise1D)
                    {
                        noisePowerPos = DuVector3.New(duNoisePos.Perlin1D(randomVector.x));

                        if (synchronized)
                        {
                            noisePowerRot = noisePowerScl = noisePowerPos;
                        }
                        else
                        {
                            noisePowerRot = DuVector3.New(duNoiseRot.Perlin1D(randomVector.y));
                            noisePowerScl = DuVector3.New(duNoiseScl.Perlin1D(randomVector.z));
                        }
                    }
                    else // NoiseDimension.Noise3D
                    {
                        noisePowerPos = duNoisePos.Perlin1D_asVector3(randomVector.x);

                        if (synchronized)
                        {
                            noisePowerRot = noisePowerScl = noisePowerPos;
                        }
                        else
                        {
                            noisePowerRot = duNoiseRot.Perlin1D_asVector3(randomVector.y);
                            noisePowerScl = duNoiseScl.Perlin1D_asVector3(randomVector.z);
                        }
                    }
                    break;
                }

                case NoiseMode.Perlin:
                {
                    float animTotalOffset = m_OffsetDynamic + animationOffset * animationSpeed;
                    Vector3 inSpaceOffset;

                    switch (noiseSpace)
                    {
                        case NoiseSpace.Global:
                            inSpaceOffset = factoryInstanceState.factory.GetPositionInWorldSpace(factoryInstance);
                            break;

                        case NoiseSpace.Local:
                        default:
                            inSpaceOffset = factoryInstance.stateDynamic.position;
                            break;
                    }

                    if (DuMath.IsNotZero(noiseScale))
                        inSpaceOffset /= noiseScale;

                    if (noiseDimension == NoiseDimension.Noise1D)
                    {
                        noisePowerPos = DuVector3.New(duNoisePos.Perlin3D(inSpaceOffset, animTotalOffset));

                        if (synchronized)
                        {
                            noisePowerRot = noisePowerScl = noisePowerPos;
                        }
                        else
                        {
                            noisePowerRot = DuVector3.New(duNoiseRot.Perlin3D(inSpaceOffset, animTotalOffset));
                            noisePowerScl = DuVector3.New(duNoiseScl.Perlin3D(inSpaceOffset, animTotalOffset));
                        }
                    }
                    else // NoiseDimension.Noise3D
                    {
                        noisePowerPos = duNoisePos.Perlin3D_asVector3(inSpaceOffset, animTotalOffset);

                        if (synchronized)
                        {
                            noisePowerRot = noisePowerScl = noisePowerPos;
                        }
                        else
                        {
                            noisePowerRot = duNoiseRot.Perlin3D_asVector3(inSpaceOffset, animTotalOffset);
                            noisePowerScl = duNoiseScl.Perlin3D_asVector3(inSpaceOffset, animTotalOffset);
                        }
                    }
                    break;
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            noisePowerPos.duFit01To(min, max);
            noisePowerRot.duFit01To(min, max);
            noisePowerScl.duFit01To(min, max);

            if (synchronized && noiseDimension == NoiseDimension.Noise3D)
            {
                if (positionAxisRemapping != AxisRemapping.Off)
                    noisePowerPos = AxisRemap(noisePowerPos, positionAxisRemapping);

                if (rotationAxisRemapping != AxisRemapping.Off)
                    noisePowerRot = AxisRemap(noisePowerRot, rotationAxisRemapping);

                if (scaleAxisRemapping != AxisRemapping.Off)
                    noisePowerScl = AxisRemap(noisePowerScl, scaleAxisRemapping);
            }

            factoryInstanceState.extraIntensityEnabled = true;
            factoryInstanceState.extraIntensityPosition = noisePowerPos;
            factoryInstanceState.extraIntensityRotation = noisePowerRot;
            factoryInstanceState.extraIntensityScale = noisePowerScl;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            UpdateInstanceDynamicState(factoryInstanceState, intensity);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Vector3 AxisRemap(Vector3 v, AxisRemapping axisRemapping)
        {
            switch (axisRemapping)
            {
                case AxisRemapping.Off: break;
                case AxisRemapping.XyzToXzy: v = new Vector3(v.x, v.z, v.y); break;
                case AxisRemapping.XyzToYxz: v = new Vector3(v.y, v.x, v.z); break;
                case AxisRemapping.XyzToYzx: v = new Vector3(v.y, v.z, v.x); break;
                case AxisRemapping.XyzToZxy: v = new Vector3(v.z, v.x, v.y); break;
                case AxisRemapping.XyzToZyx: v = new Vector3(v.z, v.y, v.x); break;
            }
            return v;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, noiseMode);
            DuDynamicState.Append(ref dynamicState, ++seq, noiseDimension);
            DuDynamicState.Append(ref dynamicState, ++seq, animationSpeed);
            DuDynamicState.Append(ref dynamicState, ++seq, animationOffset);
            DuDynamicState.Append(ref dynamicState, ++seq, synchronized);

            DuDynamicState.Append(ref dynamicState, ++seq, positionAxisRemapping);
            DuDynamicState.Append(ref dynamicState, ++seq, rotationAxisRemapping);
            DuDynamicState.Append(ref dynamicState, ++seq, scaleAxisRemapping);

            DuDynamicState.Append(ref dynamicState, ++seq, noiseSpace);
            DuDynamicState.Append(ref dynamicState, ++seq, noiseScale);

            DuDynamicState.Append(ref dynamicState, ++seq, seed);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            min = -1f;
            ResetStates();
        }

        public void ResetStates()
        {
            m_DuNoisePos = null;
            m_DuNoiseRot = null;
            m_DuNoiseScl = null;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Normalizer

        public static class Normalizer
        {
            public static float AnimationSpeed(float value)
            {
                return Mathf.Clamp(value, 0f, float.MaxValue);
            }

            public static float NoiseScale(float value)
            {
                return Mathf.Clamp(value, 0.0001f, float.MaxValue);
            }
        }
    }
}
