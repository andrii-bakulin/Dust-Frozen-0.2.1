using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Action RotateTo")]
    public class DuActionRotateTo : DuIntervalAction
    {
        public enum Space
        {
            World = 0,
            Local = 1,
        }

        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Vector3 m_EndRotation = Vector3.zero;
        public Vector3 endRotation
        {
            get => m_EndRotation;
            set => m_EndRotation = value;
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

        internal override void OnActionUpdate(float deltaTime)
        {
            Transform tr = GetTargetTransform();

            if (Dust.IsNull(tr))
                return;

            var lerpOffset = duration > 0f && percentsCompletedNow < 1f
                ? deltaTime / ((1f - percentsCompletedNow) * duration)
                : 1f;

            Quaternion quaEndRotation = Quaternion.Euler(endRotation);

            switch (space)
            {
                case Space.World:
                    tr.rotation = Quaternion.Lerp(tr.rotation, quaEndRotation, lerpOffset);
                    break;

                case Space.Local:
                    tr.localRotation = Quaternion.Lerp(tr.localRotation, quaEndRotation, lerpOffset);
                    break;
            }
        }
    }
}
