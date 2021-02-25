using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Flip Action")]
    public class DuFlipAction : DuInstantAction
    {
        [SerializeField]
        private bool m_FlipX = false;
        public bool flipX
        {
            get => m_FlipX;
            set => m_FlipX = value;
        }

        [SerializeField]
        private bool m_FlipY = false;
        public bool flipY
        {
            get => m_FlipY;
            set => m_FlipY = value;
        }

        [SerializeField]
        private bool m_FlipZ = false;
        public bool flipZ
        {
            get => m_FlipZ;
            set => m_FlipZ = value;
        }

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        internal override void OnActionUpdate(float deltaTime)
        {
            Transform tr = GetTargetTransform();

            if (Dust.IsNull(tr))
                return;

            Vector3 scale = tr.localScale;

            if (flipX) scale.x *= -1f;
            if (flipY) scale.y *= -1f;
            if (flipZ) scale.z *= -1f;

            tr.localScale = scale;
        }
    }
}
