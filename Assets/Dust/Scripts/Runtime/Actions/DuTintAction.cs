using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Tint Action")]
    public class DuTintAction : DuIntervalWithRollbackAction
    {
        public abstract class TintUpdater
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
            
            m_ActiveTintUpdater = FactoryUpdater(tintMode);
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

        //--------------------------------------------------------------------------------------------------------------
        
        private static TintMode[] autoDetectTintsSequence = new[]
        {
            TintMode.MeshRenderer,

            TintMode.UIImage,
            TintMode.UIText,
        };

        protected TintUpdater FactoryUpdater(TintMode tintMode)
        {
            switch (tintMode)
            {
                case TintMode.Auto:
                    foreach (var tryTintMode in autoDetectTintsSequence)
                    {
                        TintUpdater updater = FactoryUpdater(tryTintMode);

                        if (Dust.IsNotNull(updater))
                            return updater;
                    }
                    return null;
                    
                case TintMode.MeshRenderer:
                    return DuMeshRendererTintUpdater.Create(this);

                case TintMode.UIImage:
                    return DuUIImageTintUpdater.Create(this);
                case TintMode.UIText:
                    return DuUITextTintUpdater.Create(this);
            }
            return null;
        }
    }
}
