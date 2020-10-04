using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    public abstract partial class DuFactory : DuMonoBehaviour
    {
        public enum IterateMode
        {
            Iterate = 0,
            Random = 1,
        }

        public enum InstanceMode
        {
            Inherit = 0,
            UnpackPrefabs = 1,
        }

        public enum Orientation
        {
            XY = 0,
            ZY = 1,
            XZ = 2,
        }

        public enum TransformSpace
        {
            Factory = 0,
            Instance = 1,
        }

        public enum TransformSequence
        {
            PositionRotationScale = 0,
            RotationPositionScale = 1,
        }

        public enum InspectorDisplay
        {
            None = 0,
            Value = 1,
            Color = 2,
            Index = 3,
            UV = 4,
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
        private InstanceMode m_InstanceMode = InstanceMode.Inherit;
        public InstanceMode instanceMode
        {
            get => m_InstanceMode;
            set => m_InstanceMode = value;
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
        private Color m_DefaultColor = Color.white;
        public Color defaultColor
        {
            get => m_DefaultColor;
            set => m_DefaultColor = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private InspectorDisplay m_InspectorDisplay = InspectorDisplay.None;
        public InspectorDisplay inspectorDisplay => m_InspectorDisplay;

        [SerializeField]
        private float m_InspectorScale = 1f;
        public float inspectorScale => m_InspectorScale;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private List<GameObject> m_Objects = new List<GameObject>();
        public List<GameObject> objects => m_Objects;

        [SerializeField]
        private List<DuFactoryMachine.Record> m_FactoryMachines = new List<DuFactoryMachine.Record>();
        public List<DuFactoryMachine.Record> factoryMachines => m_FactoryMachines;

        // Why I use array[] and not List<> ?
        //   1. array[] faster then iterate (+ need iterate via for, not foreach)
        //   2. I always know capacity of instances
        [SerializeField]
        private DuFactoryInstance[] m_Instances = new DuFactoryInstance[0]; // shouldn't be null
        public DuFactoryInstance[] instances => m_Instances;

        //--------------------------------------------------------------------------------------------------------------

        public abstract string FactoryName();

        //--------------------------------------------------------------------------------------------------------------

        public Vector3 GetInstancePositionInWorldSpace(DuFactoryInstance factoryInstance)
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

        void Reset()
        {
            DestroyAllInstances();
        }
    }
}
