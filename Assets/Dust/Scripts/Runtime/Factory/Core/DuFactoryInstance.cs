using System;
using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Support/Factory Instance")]
    public class DuFactoryInstance : DuMonoBehaviour
    {
        [System.Serializable]
        public class State
        {
            public Vector3 position = Vector3.zero;
            public Vector3 rotation = Vector3.zero;
            public Vector3 scale = Vector3.one;

            public float value = 0f;
            public Color color = Color.magenta;
            public Vector3 uvw = Vector3.zero;

            public void ApplyToTransform(Transform t)
            {
                t.localPosition = position;
                t.localEulerAngles = rotation;
                t.localScale = scale;
            }

            public State Clone()
            {
                var clone = new State
                {
                    position = this.position,
                    rotation = this.rotation,
                    scale    = this.scale,

                    value    = this.value,
                    color    = this.color,
                    uvw      = this.uvw,
                };
                return clone;
            }

            public void CopyFrom(State t)
            {
                this.position = t.position;
                this.rotation = t.rotation;
                this.scale    = t.scale;

                this.value    = t.value;
                this.color    = t.color;
                this.uvw      = t.uvw;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [Serializable]
        public class MaterialReference
        {
            [SerializeField]
            private MeshRenderer m_MeshRenderer;
            public MeshRenderer meshRenderer
            {
                get => m_MeshRenderer;
                set => m_MeshRenderer = value;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            [SerializeField]
            private string m_ValuePropertyName = "";
            public string valuePropertyName
            {
                get => m_ValuePropertyName;
                set => m_ValuePropertyName = value;
            }

            [SerializeField]
            private string m_ColorPropertyName = "_Color";
            public string colorPropertyName
            {
                get => m_ColorPropertyName;
                set => m_ColorPropertyName = value;
            }

            [SerializeField]
            private string m_UvwPropertyName = "";
            public string uvwPropertyName
            {
                get => m_UvwPropertyName;
                set => m_UvwPropertyName = value;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            [SerializeField]
            private Material m_OriginalMaterial;
            public Material originalMaterial
            {
                get => m_OriginalMaterial;
                set
                {
                    if (Dust.IsNotNull(value))
                    {
                        if (Dust.IsNull(m_OriginalMaterial))
                            m_OriginalMaterial = value;
                    }
                    else
                        m_OriginalMaterial = null;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        internal DuFactory m_ParentFactory = null;
        public DuFactory parentFactory => m_ParentFactory;

        [SerializeField]
        private DuFactoryInstance m_PrevInstance = null;
        public DuFactoryInstance prevInstance => m_PrevInstance;

        [SerializeField]
        private DuFactoryInstance m_NextInstance = null;
        public DuFactoryInstance nextInstance => m_NextInstance;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Index;
        public int index => m_Index;

        // Instance offset in total instances sequence.
        // Value in range [0..1]
        // First instance is 0.0
        // Last instance is 1.0
        [SerializeField]
        private float m_Offset;
        public float offset => m_Offset;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private State m_StateZero = new State();
        public State stateZero => m_StateZero;

        private readonly State m_StateDynamic = new State();
        public State stateDynamic => m_StateDynamic;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Material reference(s)

        // Why I use this bool flag and not check ..if (Dust.IsNull(m_MaterialReference))..
        // Because if developer enable debug mode from inspector and click on factory instance
        // Then Inspector auto-create m_MaterialReference object.
        // But it'll have from values and will not be inserted to references array
        private bool m_MaterialReferenceLinked = false;

        // No need to serialize field.
        // It's just link to m_MaterialReferences[0]
        private MaterialReference m_MaterialReference;
        public MaterialReference materialReference
        {
            get
            {
                if (!m_MaterialReferenceLinked)
                {
                    if (materialReferences.Count == 0)
                        materialReferences.Add(GetDefaultMaterialReference());

                    m_MaterialReference = materialReferences[0];
                    m_MaterialReferenceLinked = true;
                }

                return m_MaterialReference;
            }
        }

        // @DUST.todo: Now I use only 1st element of the array as main reference to material.
        // But in future I can add more references for few MeshRender:Materials + specific params
        [SerializeField]
        private List<MaterialReference> m_MaterialReferences = null;
        private List<MaterialReference> materialReferences
        {
            get
            {
                if (Dust.IsNull(m_MaterialReferences))
                    m_MaterialReferences = new List<MaterialReference>();

                return m_MaterialReferences;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Initialize(DuFactory duFactory, int initIndex, float initOffset)
        {
            m_ParentFactory = duFactory;
            m_Index = initIndex;
            m_Offset = initOffset;
        }

        internal void SetPrevNextInstances(DuFactoryInstance prevFactoryInstance, DuFactoryInstance nextFactoryInstance)
        {
            m_PrevInstance = prevFactoryInstance;
            m_NextInstance = nextFactoryInstance;
        }

        public void SetDefaultState(State state)
        {
            m_StateZero.CopyFrom(state);
        }

        //--------------------------------------------------------------------------------------------------------------

        private bool m_DidApplyMaterialUpdatesBefore = false;
        private bool m_DidApplyMaterialUpdatesLastIteration = false;

        internal void ResetDynamicStateToZeroState()
        {
            m_StateDynamic.CopyFrom(m_StateZero);

            m_DidApplyMaterialUpdatesLastIteration = false;
        }

        internal void ApplyDynamicStateToObject()
        {
            m_StateDynamic.ApplyToTransform(this.transform);

            if (m_DidApplyMaterialUpdatesBefore && !m_DidApplyMaterialUpdatesLastIteration)
            {
                var matRef = materialReference;

                if( Dust.IsNotNull(matRef.originalMaterial))
                {
                    matRef.meshRenderer.sharedMaterial = matRef.originalMaterial;
                    matRef.originalMaterial = null;
                }

                m_DidApplyMaterialUpdatesBefore = false;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        internal void ApplyMaterialUpdatesToObject(float intensity)
        {
            var matRef = materialReference;

            if (Dust.IsNull(matRef.meshRenderer))
                return;

            Material material;

            if (Dust.IsNull(matRef.originalMaterial))
            {
                if (Dust.IsNull(matRef.meshRenderer.sharedMaterial))
                    return;

                matRef.originalMaterial = matRef.meshRenderer.sharedMaterial;

                material = new Material(matRef.originalMaterial);
                material.name += " (Clone)";

                // Creating clone of material
                matRef.meshRenderer.sharedMaterial = material;
            }
            else
            {
                material = matRef.meshRenderer.sharedMaterial;
            }

            if (!Dust.IsNullOrEmpty(matRef.valuePropertyName))
                material.SetFloat(matRef.valuePropertyName, stateDynamic.value * intensity);

            if (!Dust.IsNullOrEmpty(matRef.colorPropertyName))
                material.SetColor(matRef.colorPropertyName, stateDynamic.color * intensity);

            if (!Dust.IsNullOrEmpty(matRef.uvwPropertyName))
                material.SetVector(matRef.uvwPropertyName, stateDynamic.uvw * intensity);

            m_DidApplyMaterialUpdatesBefore = true;
            m_DidApplyMaterialUpdatesLastIteration = true;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public MaterialReference GetDefaultMaterialReference()
        {
            var matRef = new MaterialReference();

            matRef.meshRenderer = GetComponent<MeshRenderer>();

            if (Dust.IsNull(matRef.meshRenderer))
                matRef.meshRenderer = GetComponentInChildren<MeshRenderer>();

            return matRef;
        }
    }
}
