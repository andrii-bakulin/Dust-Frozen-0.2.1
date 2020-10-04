using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Time Transform Machine")]
    public class DuTimeFactoryMachine : DuFactoryPRSMachine
    {
        private float m_TimeSinceStart;
        public float timeSinceStart => m_TimeSinceStart;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Factory/Machines/Time Transform")]
        public static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuTimeFactoryMachine));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Time";
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
