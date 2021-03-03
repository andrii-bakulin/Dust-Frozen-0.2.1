using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Tint Action")]
    public partial class DuTintAction : DuIntervalWithRollbackAction
    {
        protected abstract class TintUpdater
        {
            private DuTintAction m_TintAction;
            protected DuTintAction tintAction => m_TintAction;
            
            protected Color m_StartColor;
            public Color startColor => m_StartColor;

            public virtual void Init(DuTintAction parentTintAction)
            {
                m_TintAction = parentTintAction;
            }

            public abstract void Update(float deltaTime, Color color);

            public virtual void Release(bool isActionTerminated)
            {
                m_TintAction = null;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public enum TintMode
        {
            Auto = 0,
            
            MeshRenderer = 1,

            UIImage = 101,
            UIText = 102,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Color m_TintColor = Color.white;
        public Color tintColor
        {
            get => m_TintColor;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TintColor = value;
            }
        }

        [SerializeField]
        private TintMode m_TintMode = TintMode.Auto;
        public TintMode tintMode
        {
            get => m_TintMode;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_TintMode = value;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Used by: { MeshRenderer } 

        [SerializeField]
        private string m_PropertyName = "_Color";
        public string propertyName
        {
            get => m_PropertyName;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_PropertyName = value;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected TintUpdater m_ActiveTintUpdater;

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        protected override void OnActionStart()
        {
            base.OnActionStart();
            
            if (Dust.IsNull(m_TargetTransform))
                return;

            m_ActiveTintUpdater = FactoryUpdater(this, tintMode);
        }

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_ActiveTintUpdater))
                return;

            Color color;

            if (playingPhase == PlayingPhase.Main)
                color = Color.Lerp(m_ActiveTintUpdater.startColor, tintColor, playbackState);
            else
                color = Color.Lerp(tintColor, m_ActiveTintUpdater.startColor, playbackState);
            
            m_ActiveTintUpdater.Update(deltaTime, color);
        }

        protected override void OnActionStop(bool isTerminated)
        {
            if (Dust.IsNotNull(m_ActiveTintUpdater))
            {
                m_ActiveTintUpdater.Release(isTerminated);
                m_ActiveTintUpdater = null;
            }

            base.OnActionStop(isTerminated);
        }
    }
}
