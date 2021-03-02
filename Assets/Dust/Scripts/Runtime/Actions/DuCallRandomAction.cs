using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace DustEngine
{
    [AddComponentMenu("Dust/Actions/Call Random Action")]
    public class DuCallRandomAction : DuInstantAction
    {
        [Serializable]
        public class Record
        {
            [SerializeField]
            private DuAction m_Action;
            public DuAction action
            {
                get => m_Action;
                set => m_Action = value;
            }

            [SerializeField]
            private float m_Weight = 0.5f;
            public float weight
            {
                get => m_Weight;
                set => m_Weight = Mathf.Clamp01(value);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        
        [SerializeField]
        private List<Record> m_Actions = new List<Record>();
        public List<Record> actions => m_Actions;
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed = 0;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (m_Seed == value)
                    return;

                m_Seed = value;
                m_DuRandom = null;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuRandom m_DuRandom;
        private DuRandom duRandom => m_DuRandom ?? (m_DuRandom = new DuRandom(seed));

        //--------------------------------------------------------------------------------------------------------------
        // DuAction lifecycle

        protected override void OnActionUpdate(float deltaTime)
        {
            if (Dust.IsNull(m_TargetTransform))
                return;

            if (actions.Count == 0)
                return;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Calculate Weight

            var totalWeight = 0f;

            foreach (var actionRecord in actions)
            {
                if (Dust.IsNull(actionRecord))
                    continue;
                
                totalWeight += actionRecord.weight;
            }
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // If ALL are ZERO -> just call random with same weights

            if (DuMath.IsZero(totalWeight))
            {
                var actionRecord = actions[duRandom.Range(0, actions.Count)];
                
                if (Dust.IsNotNull(actionRecord) && Dust.IsNotNull(actionRecord.action))
                    actionRecord.action.Play();
                
                return;
            }
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Choose

            var randomWeight = duRandom.Range(0f, totalWeight);
            
            foreach (var actionRecord in actions)
            {
                if (Dust.IsNull(actionRecord))
                    continue;

                if (randomWeight > actionRecord.weight)
                {
                    randomWeight -= actionRecord.weight;
                    continue;
                }

                if (Dust.IsNotNull(actionRecord.action))
                    actionRecord.action.Play();

                break;
            }
        }
    }
}
