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

        [SerializeField]
        private bool m_Position = true;
        public bool position
        {
            get => m_Position;
            set => m_Position = value;
        }

        [SerializeField]
        private bool m_Rotation = true;
        public bool rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        [SerializeField]
        private bool m_Scale = true;
        public bool scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(sourceObject) || sourceObject.Equals(this.gameObject))
                return;

            Transform tr = this.transform;

            switch (space)
            {
                case Space.World:
                    if (position)
                        tr.position = sourceObject.transform.position;

                    if (rotation)
                        tr.rotation = sourceObject.transform.rotation;

                    if (scale)
                    {
                        tr.localScale = Vector3.one;

                        Vector3 newScale = sourceObject.transform.lossyScale;
                        Vector3 curScale = tr.lossyScale;

                        tr.localScale = new Vector3(newScale.x / curScale.x, newScale.y / curScale.y, newScale.z / curScale.z);
                    }
                    break;

                case Space.Local:
                    if (position)
                        tr.localPosition = sourceObject.transform.localPosition;

                    if (rotation)
                        tr.localRotation = sourceObject.transform.localRotation;

                    if (scale)
                        tr.localScale = sourceObject.transform.localScale;
                    break;
            }
        }
    }
}
