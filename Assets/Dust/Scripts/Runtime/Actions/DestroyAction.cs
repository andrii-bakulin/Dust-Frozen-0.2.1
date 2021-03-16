using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Destroy Action")]
    public class DestroyAction : InstantAction
    {
        [SerializeField]
        private bool m_DisableColliders = true;
        public bool disableColliders
        {
            get => m_DisableColliders;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_DisableColliders = value;
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            // Cannot call DestroyNow() here, because then callbacks will be ignored
            // So require call it as a last instruction in ActionInnerStop method! 
        }

        protected override void ActionInnerStop(bool isTerminated)
        {
            base.ActionInnerStop(isTerminated);

            DestroyNow();
        }

        public void DestroyNow()
        {
            if (Dust.IsNull(activeTargetObject))
                return;

            if (disableColliders)
            {
                Collider[] objColliders = activeTargetObject.GetComponents<Collider>();

                foreach (var objCollider in objColliders)
                {
                    objCollider.enabled = false;
                }
            }

            Destroy(activeTargetObject);
        }
    }
}
