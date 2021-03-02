using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Transform Copy Action")]
    public class DuTransformCopyAction : DuInstantAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_Position = false;
        public bool position
        {
            get => m_Position;
            set => m_Position = value;
        }

        [SerializeField]
        private bool m_Rotation = false;
        public bool rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        [SerializeField]
        private bool m_Scale = false;
        public bool scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private GameObject m_SourceObject;
        public GameObject sourceObject
        {
            get => m_SourceObject;
            set => m_SourceObject = value;
        }

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(sourceObject) || sourceObject.Equals(this.gameObject))
                return;

            if (Dust.IsNull(m_TargetTransform))
                return;

            if (space == Space.World)
            {
                if (position)
                    m_TargetTransform.position = sourceObject.transform.position;

                if (rotation)
                    m_TargetTransform.rotation = sourceObject.transform.rotation;

                if (scale)
                    DuTransform.SetGlobalScale(m_TargetTransform, sourceObject.transform.lossyScale);
            }
            else if (space == Space.Local)
            {
                if (position)
                    m_TargetTransform.localPosition = sourceObject.transform.localPosition;

                if (rotation)
                    m_TargetTransform.localRotation = sourceObject.transform.localRotation;

                if (scale)
                    m_TargetTransform.localScale = sourceObject.transform.localScale;
            }
        }
    }
}
