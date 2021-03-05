using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Destroyer Action")]
    public class DuDestroyerAction : DuInstantAction
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
        // DuAction lifecycle

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
            GameObject target = GetTargetObject();

            if (Dust.IsNull(target))
                return;

            if (disableColliders)
            {
                Collider[] objColliders = target.GetComponents<Collider>();

                foreach (var objCollider in objColliders)
                {
                    objCollider.enabled = false;
                }
            }

            Destroy(target.gameObject);
        }
    }
}
