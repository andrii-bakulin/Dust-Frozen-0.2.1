using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public abstract class DuFactoryBuilder
    {
        protected DuFactory m_DuFactory;

        protected List<DuFactoryInstance.State> m_InstancesStates;

        public virtual void Initialize(DuFactory duFactory)
        {
            m_DuFactory = duFactory;

            m_InstancesStates = new List<DuFactoryInstance.State>();
        }

        // Alias for Initialize, but in some cases logic maybe much simpler
        public virtual void Reinitialize(DuFactory duFactory)
        {
            Initialize(duFactory);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Instance Manager

        internal DuFactoryInstance CreateFactoryInstance(int instanceIndex, int instancesCount)
        {
            GameObject prefab = ObjectsQueue_GetNextPrefab();

            if (Dust.IsNull(prefab))
                return null;

            GameObject instanceGameObject;

#if UNITY_EDITOR
            if (m_DuFactory.instanceMode == DuFactory.InstanceMode.Inherit && Dust.IsPrefab(prefab))
            {
                instanceGameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                if (Dust.IsNull(instanceGameObject))
                    return null;

                instanceGameObject.transform.parent = m_DuFactory.transform;
            }
            else
#endif
            {
                instanceGameObject = Object.Instantiate(prefab, m_DuFactory.transform);
            }

            if (Dust.IsNull(instanceGameObject))
                return null;

            if (m_DuFactory.forcedSetActive)
                instanceGameObject.SetActive(true);

            DuFactoryInstance duFactoryInstance = instanceGameObject.GetComponent<DuFactoryInstance>();

            if (Dust.IsNull(duFactoryInstance))
                duFactoryInstance = instanceGameObject.AddComponent<DuFactoryInstance>();

            float instanceOffset = instancesCount > 1 ? (float) instanceIndex / (instancesCount - 1) : 0f;

            duFactoryInstance.Initialize(m_DuFactory, instanceIndex, instanceOffset);

            return duFactoryInstance;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Objects Queue

        private int m_ObjectsQueue_index;
        private DuRandom m_ObjectsQueue_duRandom;

        internal void ObjectsQueue_Initialize()
        {
            switch (m_DuFactory.iterateMode)
            {
                case DuFactory.IterateMode.Iterate:
                    m_ObjectsQueue_index = 0;
                    break;

                case DuFactory.IterateMode.Random:
                    m_ObjectsQueue_duRandom = new DuRandom(Mathf.Max(m_DuFactory.seed, 1));
                    break;
            }
        }

        private GameObject ObjectsQueue_GetNextPrefab()
        {
            if (m_DuFactory.objects.Count == 0)
                return null;

            switch (m_DuFactory.iterateMode)
            {
                case DuFactory.IterateMode.Iterate:
                    return m_DuFactory.objects[(m_ObjectsQueue_index++) % m_DuFactory.objects.Count];

                case DuFactory.IterateMode.Random:
                    return m_DuFactory.objects[m_ObjectsQueue_duRandom.Range(0, m_DuFactory.objects.Count)];

                default:
                    return null;
            }
        }

        internal void ObjectsQueue_Release()
        {
            m_ObjectsQueue_duRandom = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual int GetTotalInstancesCount()
        {
            return m_InstancesStates.Count;
        }

        // This method should calculate:
        // - position, rotation, scale
        // - UVW
        // Values for (next) params will be defined by instance:
        // - value
        // - color
        public virtual DuFactoryInstance.State GetDefaultInstanceState(DuFactoryInstance duFactoryInstance)
        {
            return m_InstancesStates[duFactoryInstance.index].Clone();
        }
    }
}
