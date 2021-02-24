using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Transform Set Action")]
    public class DuTransformSetAction : DuInstantAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private bool m_AssignPosition = false;
        public bool assignPosition
        {
            get => m_AssignPosition;
            set => m_AssignPosition = value;
        }

        [SerializeField]
        private bool m_AssignRotation = false;
        public bool assignRotation
        {
            get => m_AssignRotation;
            set => m_AssignRotation = value;
        }

        [SerializeField]
        private bool m_AssignScale = false;
        public bool assignScale
        {
            get => m_AssignScale;
            set => m_AssignScale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;
        public Vector3 position
        {
            get => m_Position;
            set => m_Position = value;
        }

        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;
        public Vector3 rotation
        {
            get => m_Rotation;
            set => m_Rotation = value;
        }

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        public Vector3 scale
        {
            get => m_Scale;
            set => m_Scale = value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private Space m_Space = Space.Local;
        public Space space
        {
            get => m_Space;
            set => m_Space = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionUpdate(float deltaTime)
        {
            Transform tr = this.transform;

            if (space == Space.World)
            {
                if (assignPosition)
                    tr.position = position;

                if (assignRotation)
                    tr.rotation = Quaternion.Euler(rotation);

                if (assignScale)
                    DuTransform.SetGlobalScale(tr, scale);
            }
            else if (space == Space.Local)
            {
                if (assignPosition)
                    tr.localPosition = position;

                if (assignRotation)
                    tr.localRotation = Quaternion.Euler(rotation);

                if (assignScale)
                    tr.localScale = scale;
            }
        }
    }
}
