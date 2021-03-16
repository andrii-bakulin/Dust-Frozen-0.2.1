using System.Collections.Generic;
using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Activate Action")]
    public class DuActivateAction : DuInstantAction
    {
        public enum Action
        {
            Disable = 0,
            Enable = 1,
            Toggle = 2,
            ToggleRandom = 3,
        }
        
        //--------------------------------------------------------------------------------------------------------------

        [SerializeField]
        private Action m_Action = Action.Enable;
        public Action action
        {
            get => m_Action;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_Action = value;
            }
        }

        [SerializeField]
        private bool m_ApplyToSelf = false;
        public bool applyToSelf
        {
            get => m_ApplyToSelf;
            set
            {
                if (!IsAllowUpdateProperty()) return;
                m_ApplyToSelf = value;
            }
        }

        [SerializeField]
        private int m_Seed = 0;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (!IsAllowUpdateProperty()) return;

                if (m_Seed == value)
                    return;

                m_Seed = value;
                m_DuRandom = null;
            }
        }

        [SerializeField]
        private List<GameObject> m_GameObjects = new List<GameObject>();
        public List<GameObject> gameObjects => m_GameObjects;

        [SerializeField]
        private List<Component> m_Components = new List<Component>();
        public List<Component> components => m_Components;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuRandom m_DuRandom;
        private DuRandom duRandom => m_DuRandom ?? (m_DuRandom = new DuRandom(seed));

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(activeTargetTransform))
                return;
            
            if (applyToSelf)
                gameObject.SetActive(GetNewState(gameObject.activeSelf));

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (Dust.IsNull(gameObjects[i]))
                    continue;
                
                gameObjects[i].SetActive(GetNewState(gameObjects[i].activeSelf));
            }

            for (int i = 0; i < components.Count; i++)
            {
                Component comp = components[i];

                if (Dust.IsNull(comp))
                    continue;

                if (comp as Behaviour is Behaviour compBehaviour)
                {
                    compBehaviour.enabled = GetNewState(compBehaviour.enabled);
                }
                else if (comp as Collider is Collider compCollider)
                {
                    compCollider.enabled = GetNewState(compCollider.enabled);
                }
                else if (comp as Renderer is Renderer compRenderer)
                {
                    compRenderer.enabled = GetNewState(compRenderer.enabled);
                }
                else if (comp as Cloth is Cloth compCloth)
                {
                    compCloth.enabled = GetNewState(compCloth.enabled);
                }
                else if (comp as LODGroup is LODGroup compLODGroup)
                {
                    compLODGroup.enabled = GetNewState(compLODGroup.enabled);
                }
                
                // Next classes have no 'enabled' property:
                // - CanvasRenderer
                // - Joint
                // - MeshFilter
                // - OcclusionArea
                // - OcclusionPortal
                // - ParticleSystem
                // - ParticleSystemForceField
                // - Rigidbody
                // - Rigidbody2D
                // - TextMesh
                // - Transform
                // - Tree
                // - WindZone
                // - XR.WSA.WorldAnchor
                //
                // To find classes inherited from Component find next in Unity Documentation:
                // Inherits from:<a href="Component.html" class="cl">Component</a>
            }
        }

        protected bool GetNewState(bool currentState)
        {
            switch (action)
            {
                case Action.Disable:       return false;
                case Action.Enable:        return true;
                case Action.Toggle:        return !currentState;
                case Action.ToggleRandom:  return duRandom.Next() >= 0.5f;
                default:                   return currentState;
            }
        }
    }
}
