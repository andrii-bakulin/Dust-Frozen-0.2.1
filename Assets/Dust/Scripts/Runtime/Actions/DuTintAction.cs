using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Tint Action")]
    public class DuTintAction : DuIntervalAction
    {
        [SerializeField]
        private MeshRenderer m_MeshRenderer;
        public MeshRenderer meshRenderer
        {
            get => m_MeshRenderer;
            set => m_MeshRenderer = value;
        }

        [SerializeField]
        private Color m_TintColor = Color.white;
        public Color tintColor
        {
            get => m_TintColor;
            set => m_TintColor = value;
        }

        [SerializeField]
        private string m_PropertyName = "_Color";
        public string propertyName
        {
            get => m_PropertyName;
            set => m_PropertyName = value;
        }

        //--------------------------------------------------------------------------------------------------------------

        private Material m_OriginalMaterial;
        private Material m_TintMaterial;

        private Color m_ColorStartFrom;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionStart()
        {
            base.OnActionStart();
            
            if (Dust.IsNull(m_MeshRenderer))
                return;

            m_OriginalMaterial = m_MeshRenderer.sharedMaterial;
            
            m_TintMaterial = new Material(m_OriginalMaterial);
            m_TintMaterial.hideFlags = HideFlags.DontSave;

            m_ColorStartFrom = m_TintMaterial.GetColor(propertyName);

            m_MeshRenderer.sharedMaterial = m_TintMaterial;
        }

        internal override void OnActionStop(bool isTerminated)
        {
            // If need return material back!
            // m_MeshRenderer.sharedMaterial = m_OriginalMaterial;
            
            m_OriginalMaterial = null;
            m_TintMaterial = null;

            base.OnActionStop(isTerminated);
        }

        internal override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TintMaterial))
                return;

            m_TintMaterial.SetColor(propertyName, Color.Lerp(m_ColorStartFrom, tintColor, percentsCompletedNow));
        }
        
        //--------------------------------------------------------------------------------------------------------------

        protected override void ResetStates()
        {
            base.ResetStates();

            meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}
