using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Material Machine")]
    public class DuMaterialFactoryMachine : DuFactoryMachine
    {
#if UNITY_EDITOR
        [MenuItem("Dust/Factory/Machines/Material")]
        public static void AddComponent()
        {
            AddFactoryMachineComponentByType(typeof(DuMaterialFactoryMachine));
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public override string FactoryMachineName()
        {
            return "Material";
        }

#if UNITY_EDITOR
        public override string FactoryMachineDynamicHint()
        {
            return "";
        }
#endif

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public override bool PrepareForUpdateInstancesStates(FactoryInstanceState factoryInstanceState)
        {
            // Should execute logic even if intensityByFactory is ZERO
            return true;
        }

        public override void UpdateInstanceState(FactoryInstanceState factoryInstanceState)
        {
            factoryInstanceState.instance.ApplyMaterialUpdatesToObject(factoryInstanceState.intensityByFactory * intensity);
        }

        public override void FinalizeUpdateInstancesStates(FactoryInstanceState factoryInstanceState)
        {
        }
    }
}
