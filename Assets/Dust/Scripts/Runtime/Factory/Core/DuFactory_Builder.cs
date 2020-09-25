using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public partial class DuFactory
    {
        private DuFactoryBuilder m_Builder;

        internal DuFactoryBuilder builder
        {
            get
            {
                if (Dust.IsNull(m_Builder))
                {
                    m_Builder = GetFactoryBuilder();
                    m_Builder.Initialize(this);
                }

                return m_Builder;
            }
        }

        internal abstract DuFactoryBuilder GetFactoryBuilder();

        private void DestroyBuilder()
        {
            m_Builder = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        public void RebuildInstances()
        {
            DestroyAllInstances();
            DestroyBuilder();

            // @notice: on get builder -> it'll auto create it!
            if (Dust.IsNull(builder))
                return;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Instantiate All Instances + Ignore NULL elements + finally repack to array

            var instancesPacked = new List<DuFactoryInstance>();

            builder.ObjectsQueue_Initialize();

            int instancesCount = builder.GetTotalInstancesCount();
            for (int instanceIndex = 0; instanceIndex < instancesCount; instanceIndex++)
            {
                var instance = builder.CreateFactoryInstance(instanceIndex, instancesCount);

                if (Dust.IsNull(instance))
                    continue;

                instancesPacked.Add(instance);
            }

            builder.ObjectsQueue_Release();

            m_Instances = instancesPacked.ToArray();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Create prev/next instance refernces

            for (int i = 0; i < m_Instances.Length; i++)
            {
                DuFactoryInstance prevInstance = i > 0 ? m_Instances[i - 1] : null;
                DuFactoryInstance nextInstance = i < m_Instances.Length - 1 ? m_Instances[i + 1] : null;

                m_Instances[i].SetPrevNextInstances(prevInstance, nextInstance);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            UpdateInstancesZeroStates();
        }

        protected void DestroyAllInstances()
        {
            for (int i = 0; i < transform.childCount;)
            {
                DuFactoryInstance instance = transform.GetChild(i).gameObject.GetComponent<DuFactoryInstance>();

                if (Dust.IsNull(instance))
                {
                    i++;
                    continue;
                }

                if (instance.parentFactory != this)
                {
                    i++;
                    continue;
                }

                Dust.DestroyObjectWhenReady(instance.gameObject);
            }

            m_Instances = new DuFactoryInstance[0];
        }

        //--------------------------------------------------------------------------------------------------------------

        public void UpdateInstancesZeroStates()
        {
            if (Dust.IsNull(builder))
                return;

            builder.Reinitialize(this);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            int instancesCount = instances.Length;
            for (int i = 0; i < instancesCount; i++)
            {
                DuFactoryInstance factoryInstance = instances[i];

                if (Dust.IsNull(factoryInstance))
                    continue;

                var instanceState = builder.GetDefaultInstanceState(factoryInstance);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Apply instances transform params

                switch (transformSequence)
                {
                    case TransformSequence.PositionRotationScale:
                        SetInstanceZeroStates_Position(instanceState);
                        SetInstanceZeroStates_Rotation(instanceState);
                        SetInstanceZeroStates_Scale(instanceState);
                        break;

                    case TransformSequence.RotationPositionScale:
                        SetInstanceZeroStates_Rotation(instanceState);
                        SetInstanceZeroStates_Position(instanceState);
                        SetInstanceZeroStates_Scale(instanceState);
                        break;
                }

                SetInstanceZeroStates_Power(instanceState);
                SetInstanceZeroStates_Color(instanceState);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                factoryInstance.SetDefaultState(instanceState);
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            UpdateInstancesDynamicStates(true);

#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }

        private void SetInstanceZeroStates_Position(DuFactoryInstance.State instanceState)
        {
            switch (transformSpace)
            {
                case TransformSpace.Factory:
                    instanceState.position += DuMath.RotatePoint(transformPosition, instanceState.rotation);
                    break;

                case TransformSpace.Instance:
                    instanceState.position += transformPosition;
                    break;
            }
        }

        private void SetInstanceZeroStates_Rotation(DuFactoryInstance.State instanceState)
        {
            instanceState.rotation += transformRotation;
        }

        private void SetInstanceZeroStates_Scale(DuFactoryInstance.State instanceState)
        {
            instanceState.scale.Scale(transformScale);
        }

        private void SetInstanceZeroStates_Color(DuFactoryInstance.State instanceState)
        {
            instanceState.color = color;
        }

        private void SetInstanceZeroStates_Power(DuFactoryInstance.State instanceState)
        {
            instanceState.power = power;
        }
    }
}
