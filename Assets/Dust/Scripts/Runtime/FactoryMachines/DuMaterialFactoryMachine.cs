using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Factory/Machines/Material Machine")]
    public class DuMaterialFactoryMachine : DuFactoryMachine
    {
        public override string FactoryMachineName()
        {
            return "Material";
        }

        public override string FactoryMachineDynamicHint()
        {
            return "";
        }

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

        //--------------------------------------------------------------------------------------------------------------

        void Reset()
        {
            // Define default states
        }
    }
}
