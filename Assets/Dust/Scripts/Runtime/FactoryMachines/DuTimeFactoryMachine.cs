using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Time Machine")]
    public class DuTimeFactoryMachine : DuPRSFactoryMachine
    {
        private float m_TimeSinceStart;
        public float timeSinceStart => m_TimeSinceStart;

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Time";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            m_TimeSinceStart += Time.deltaTime;
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = (min + timeSinceStart * (max - min)) * intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);
        }
    }
}
