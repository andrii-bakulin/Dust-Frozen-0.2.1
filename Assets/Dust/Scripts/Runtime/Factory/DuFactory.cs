using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    public abstract partial class DuFactory : DuMonoBehaviour
    {
        internal readonly string kGameObjectName_SourceObjects = "Source Objects";
        internal readonly string kGameObjectName_Instances = "Instances";
        internal readonly string kGameObjectName_Machines = "Machines";

        public enum InspectorDisplay
        {
            None = 0,
            Value = 1,
            Color = 2,
            Index = 3,
            UV = 4,
        }

        public enum InstanceAccessMode
        {
            Normal = 0,
            NotEditable = 1,
            HideInHierarchy = 2,
        }

        public enum InstanceTypeMode
        {
            Inherit = 0,
            UnpackPrefabs = 1,
        }

        public enum IterateMode
        {
            Iterate = 0,
            Random = 1,
        }

        public enum Orientation
        {
            XY = 0,
            ZY = 1,
            XZ = 2,
        }

        public enum SourceObjectsMode
        {
            Holder = 0,
            List = 1,
            HolderAndList = 2,
        }

        public enum TransformSequence
        {
            PositionRotationScale = 0,
            RotationPositionScale = 1,
        }

        public enum TransformSpace
        {
            Factory = 0,
            Instance = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private IterateMode m_IterateMode = IterateMode.Iterate;
        public IterateMode iterateMode
        {
            get => m_IterateMode;
            set => m_IterateMode = value;
        }

        [SerializeField]
        private InstanceAccessMode m_InstanceAccessMode = InstanceAccessMode.Normal;
        public InstanceAccessMode instanceAccessMode
        {
            get => m_InstanceAccessMode;
            set => m_InstanceAccessMode = value;
        }

        [SerializeField]
        private InstanceTypeMode m_InstanceTypeMode = InstanceTypeMode.Inherit;
        public InstanceTypeMode instanceTypeMode
        {
            get => m_InstanceTypeMode;
            set => m_InstanceTypeMode = value;
        }

        [SerializeField]
        private bool m_ForcedSetActive = false;
        public bool forcedSetActive
        {
            get => m_ForcedSetActive;
            set => m_ForcedSetActive = value;
        }

        [SerializeField]
        private int m_Seed = DuConstants.RANDOM_SEED_DEFAULT;
        public int seed
        {
            get => m_Seed;
            set => m_Seed = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        [SerializeField]
        protected TransformSpace m_TransformSpace = TransformSpace.Factory;
        public TransformSpace transformSpace
        {
            get => m_TransformSpace;
            set => m_TransformSpace = value;
        }

        [SerializeField]
        protected TransformSequence m_TransformSequence = TransformSequence.PositionRotationScale;
        public TransformSequence transformSequence
        {
            get => m_TransformSequence;
            set => m_TransformSequence = value;
        }

        [SerializeField]
        protected Vector3 m_TransformPosition = Vector3.zero;
        public Vector3 transformPosition
        {
            get => m_TransformPosition;
            set => m_TransformPosition = value;
        }

        [SerializeField]
        protected Vector3 m_TransformRotation = Vector3.zero;
        public Vector3 transformRotation
        {
            get => m_TransformRotation;
            set => m_TransformRotation = value;
        }

        [SerializeField]
        protected Vector3 m_TransformScale = Vector3.one;
        public Vector3 transformScale
        {
            get => m_TransformScale;
            set => m_TransformScale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private float m_DefaultValue = 0f;
        public float defaultValue
        {
            get => m_DefaultValue;
            set => m_DefaultValue = value;
        }

        [SerializeField]
        private Color m_DefaultColor = Color.gray;
        public Color defaultColor
        {
            get => m_DefaultColor;
            set => m_DefaultColor = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private SourceObjectsMode m_SourceObjectsMode = SourceObjectsMode.HolderAndList;
        public SourceObjectsMode sourceObjectsMode
        {
            get => m_SourceObjectsMode;
            set
            {
                if (m_SourceObjectsMode == value)
                    return;

                m_SourceObjectsMode = value;
                RebuildInstances();
            }
        }

        [SerializeField]
        private GameObject m_SourceObjectsHolder;
        public GameObject sourceObjectsHolder
        {
            get => m_SourceObjectsHolder;
            set
            {
                if (m_SourceObjectsHolder == value)
                    return;

                m_SourceObjectsHolder = value;
                RebuildInstances();
            }
        }

        [SerializeField]
        private List<GameObject> m_SourceObjects = new List<GameObject>();
        public List<GameObject> sourceObjects => m_SourceObjects;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private GameObject m_InstancesHolder;
        public GameObject instancesHolder
        {
            get => m_InstancesHolder;
            set
            {
                if (m_InstancesHolder == value)
                    return;

                m_InstancesHolder = value;
                RebuildInstances();
            }
        }

        // Why I use array[] and not List<> ?
        //   1. array[] faster then iterate (+ need iterate via for, not foreach)
        //   2. I always know capacity of instances
        [SerializeField]
        private DuFactoryInstance[] m_Instances = new DuFactoryInstance[0]; // shouldn't be null
        public DuFactoryInstance[] instances => m_Instances;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private List<DuFactoryMachine.Record> m_FactoryMachines = new List<DuFactoryMachine.Record>();
        public List<DuFactoryMachine.Record> factoryMachines => m_FactoryMachines;

        [SerializeField]
        private GameObject m_FactoryMachinesHolder;
        public GameObject factoryMachinesHolder
        {
            get => m_FactoryMachinesHolder;
            set => m_FactoryMachinesHolder = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private InspectorDisplay m_InspectorDisplay = InspectorDisplay.None;
        public InspectorDisplay inspectorDisplay => m_InspectorDisplay;

        [SerializeField]
        private float m_InspectorScale = 1f;
        public float inspectorScale => m_InspectorScale;

        //--------------------------------------------------------------------------------------------------------------

        public abstract string FactoryName();

        //--------------------------------------------------------------------------------------------------------------

        public Vector3 GetPositionInWorldSpace(DuFactoryInstance factoryInstance)
        {
            return transform.TransformPoint(factoryInstance.stateDynamic.position);
        }

        public Vector3 GetPositionInWorldSpace(Vector3 localPosition)
        {
            return transform.TransformPoint(localPosition);
        }

        public Vector3 GetPositionInLocalSpace(Vector3 worldPoint)
        {
            return transform.InverseTransformPoint(worldPoint);
        }

        //--------------------------------------------------------------------------------------------------------------

        public DuFactoryMachine.Record AddFactoryMachine(DuFactoryMachine factoryMachine, float intensity = 1f, bool isEnabled = true)
        {
            var record = new DuFactoryMachine.Record
            {
                factoryMachine = factoryMachine,
                intensity = intensity,
                enabled = isEnabled,
            };

            factoryMachines.Add(record);

            return record;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            if (Application.isPlaying)
            {
                if (Dust.IsNotNull(sourceObjectsHolder))
                    sourceObjectsHolder.SetActive(false);
            }
        }

        private void Update()
        {
            RebuildInstancesIfRequired();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            UpdateInstancesDynamicStates();
        }

        private void Reset()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                if (Dust.IsNull(sourceObjectsHolder) && child.name.Equals(kGameObjectName_SourceObjects))
                {
                    sourceObjectsHolder = child.gameObject;
                }
                else if (Dust.IsNull(instancesHolder) && child.name.Equals(kGameObjectName_Instances))
                {
                    instancesHolder = child.gameObject;
                }
                else if (Dust.IsNull(factoryMachinesHolder) &&child.name.Equals(kGameObjectName_Machines))
                {
                    factoryMachinesHolder = child.gameObject;
                }
            }

            if (Dust.IsNull(sourceObjectsHolder))
            {
                sourceObjectsHolder = new GameObject();
                sourceObjectsHolder.name = kGameObjectName_SourceObjects;
                sourceObjectsHolder.transform.parent = transform;
                sourceObjectsHolder.transform.localPosition = Vector3.zero;
                sourceObjectsHolder.transform.localRotation = Quaternion.identity;
                sourceObjectsHolder.transform.localScale = Vector3.one;
            }

            if (Dust.IsNull(instancesHolder))
            {
                instancesHolder = new GameObject();
                instancesHolder.name = kGameObjectName_Instances;
                instancesHolder.transform.parent = transform;
                instancesHolder.transform.localPosition = Vector3.zero;
                instancesHolder.transform.localRotation = Quaternion.identity;
                instancesHolder.transform.localScale = Vector3.one;
            }

            if (Dust.IsNull(factoryMachinesHolder))
            {
                factoryMachinesHolder = new GameObject();
                factoryMachinesHolder.name = kGameObjectName_Machines;
                factoryMachinesHolder.transform.parent = transform;
                factoryMachinesHolder.transform.localPosition = Vector3.zero;
                factoryMachinesHolder.transform.localRotation = Quaternion.identity;
                factoryMachinesHolder.transform.localScale = Vector3.one;
            }

            DestroyAllInstances();
        }
    }
}
