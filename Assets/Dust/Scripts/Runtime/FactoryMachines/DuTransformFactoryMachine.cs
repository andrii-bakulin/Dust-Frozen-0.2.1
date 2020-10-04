using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Transform Machine")]
    public class DuTransformFactoryMachine : DuPRSFactoryMachine
    {
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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            float intensityByMachine = min + (max - min) * intensity;

            UpdateInstanceDynamicState(factoryInstanceState, intensityByMachine);
        }
    }
}
