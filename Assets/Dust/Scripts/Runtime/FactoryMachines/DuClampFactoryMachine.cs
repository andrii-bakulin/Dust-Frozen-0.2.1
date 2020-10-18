﻿using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Clamp Machine")]
    public class DuClampFactoryMachine : DuBasicFactoryMachine
    {
        public enum ClampMode
        {
            MinAndMax = 0,
            MinOnly = 1,
            MaxOnly = 2,
            NoClamp = 3,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ClampMode m_PositionMode = ClampMode.NoClamp;
        public ClampMode positionMode
        {
            get => m_PositionMode;
            set => m_PositionMode = value;
        }

        [SerializeField]
        private Vector3 m_PositionMin = Vector3.zero;
        public Vector3 positionMin
        {
            get => m_PositionMin;
            set => m_PositionMin = value;
        }

        [SerializeField]
        private Vector3 m_PositionMax = Vector3.one;
        public Vector3 positionMax
        {
            get => m_PositionMax;
            set => m_PositionMax = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ClampMode m_RotationMode = ClampMode.NoClamp;
        public ClampMode rotationMode
        {
            get => m_RotationMode;
            set => m_RotationMode = value;
        }

        [SerializeField]
        private Vector3 m_RotationMin = Vector3.zero;
        public Vector3 rotationMin
        {
            get => m_RotationMin;
            set => m_RotationMin = value;
        }

        [SerializeField]
        private Vector3 m_RotationMax = Vector3.one;
        public Vector3 rotationMax
        {
            get => m_RotationMax;
            set => m_RotationMax = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private ClampMode m_ScaleMode = ClampMode.NoClamp;
        public ClampMode scaleMode
        {
            get => m_ScaleMode;
            set => m_ScaleMode = value;
        }

        [SerializeField]
        private Vector3 m_ScaleMin = Vector3.zero;
        public Vector3 scaleMin
        {
            get => m_ScaleMin;
            set => m_ScaleMin = value;
        }

        [SerializeField]
        private Vector3 m_ScaleMax = Vector3.one;
        public Vector3 scaleMax
        {
            get => m_ScaleMax;
            set => m_ScaleMax = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Clamp";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            var instanceState = factoryInstanceState.instance.stateDynamic;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Position

            if (positionMode != ClampMode.NoClamp)
            {
                float endIntensity = factoryInstanceState.intensityByFactory
                                     * factoryInstanceState.intensityByMachine
                                     * factoryInstanceState.fieldPower;

                Vector3 endPosition = instanceState.position;

                if (positionMode == ClampMode.MinOnly || positionMode == ClampMode.MinAndMax)
                    endPosition = DuVector3.Max(endPosition, positionMin);

                if (positionMode == ClampMode.MaxOnly || positionMode == ClampMode.MinAndMax)
                    endPosition = DuVector3.Min(endPosition, positionMax);

                instanceState.position = Vector3.LerpUnclamped(instanceState.position, endPosition, endIntensity);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Rotation

            if (rotationMode != ClampMode.NoClamp)
            {
                float endIntensity = factoryInstanceState.intensityByFactory
                                     * factoryInstanceState.intensityByMachine
                                     * factoryInstanceState.fieldPower;

                Vector3 endRotation = instanceState.rotation;

                if (rotationMode == ClampMode.MinOnly || rotationMode == ClampMode.MinAndMax)
                    endRotation = DuVector3.Max(endRotation, rotationMin);

                if (rotationMode == ClampMode.MaxOnly || rotationMode == ClampMode.MinAndMax)
                    endRotation = DuVector3.Min(endRotation, rotationMax);

                instanceState.rotation = Vector3.LerpUnclamped(instanceState.rotation, endRotation, endIntensity);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Scale

            if (scaleMode != ClampMode.NoClamp)
            {
                float endIntensity = factoryInstanceState.intensityByFactory
                                     * factoryInstanceState.intensityByMachine
                                     * factoryInstanceState.fieldPower;

                Vector3 endScale = instanceState.scale;

                if (scaleMode == ClampMode.MinOnly || scaleMode == ClampMode.MinAndMax)
                    endScale = DuVector3.Max(endScale, scaleMin);

                if (scaleMode == ClampMode.MaxOnly || scaleMode == ClampMode.MinAndMax)
                    endScale = DuVector3.Min(endScale, scaleMax);

                instanceState.scale = Vector3.LerpUnclamped(instanceState.scale, endScale, endIntensity);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuDynamicStateInterface

        public override int GetDynamicStateHashCode()
        {
            var seq = 0;
            var dynamicState = base.GetDynamicStateHashCode();

            DuDynamicState.Append(ref dynamicState, ++seq, positionMode);
            DuDynamicState.Append(ref dynamicState, ++seq, positionMin);
            DuDynamicState.Append(ref dynamicState, ++seq, positionMax);

            DuDynamicState.Append(ref dynamicState, ++seq, rotationMode);
            DuDynamicState.Append(ref dynamicState, ++seq, rotationMin);
            DuDynamicState.Append(ref dynamicState, ++seq, rotationMax);

            DuDynamicState.Append(ref dynamicState, ++seq, scaleMode);
            DuDynamicState.Append(ref dynamicState, ++seq, scaleMin);
            DuDynamicState.Append(ref dynamicState, ++seq, scaleMax);

            return DuDynamicState.Normalize(dynamicState);
        }

        //--------------------------------------------------------------------------------------------------------------
    }
}
