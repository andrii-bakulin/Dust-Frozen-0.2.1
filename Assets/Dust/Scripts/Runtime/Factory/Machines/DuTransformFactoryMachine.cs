using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Transform Machine")]
    public class DuTransformFactoryMachine : DuFactoryExtendedMachine
    {
        private MachineParams m_MachineParams;

        //--------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        [MenuItem("Dust/Factory/Machines/Transform")]
        public static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuTransformFactoryMachine));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Transform";
        }

#if UNITY_EDITOR
        public override string FactoryMachineDynamicHint()
        {
            return "";
        }
#endif

        public override bool PrepareForUpdateInstancesStates(DuFactory factory, float intensityByFactory)
        {
            if (DuMath.IsZero(intensityByFactory))
                return false;

            m_MachineParams = new MachineParams();
            return true;
        }

        public override void FinalizeUpdateInstancesStates(DuFactory factory)
        {
            m_MachineParams = null;
        }

        public override void UpdateInstanceState(DuFactory factory, DuFactoryInstance factoryInstance, float intensityByFactory)
        {
            float intensityByMachine = min + (max - min) * intensity;

            m_MachineParams.factory = factory;
            m_MachineParams.factoryInstance = factoryInstance;

            m_MachineParams.intensityByFactory = intensityByFactory;
            m_MachineParams.intensityByMachine = intensityByMachine;

            UpdateInstanceDynamicState(factoryInstance.stateDynamic, m_MachineParams);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override float GetFactoryMachinePower(MachineParams machineParams)
        {
            return machineParams.intensityByMachine;
        }

        protected override Color GetFactoryMachineColor(MachineParams machineParams)
        {
            return Color.white;
        }
    }
}
