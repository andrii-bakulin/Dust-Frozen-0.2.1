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

        public override bool PrepareForUpdateInstancesStates(DuFactory factory, float intensityByFactory)
        {
            // Should execute logic even if intensityByFactory is ZERO
            return true;
        }

        public override void FinalizeUpdateInstancesStates(DuFactory factory)
        {
        }

        public override void UpdateInstanceState(DuFactory factory, DuFactoryInstance factoryInstance, float intensityByFactory)
        {
            factoryInstance.ApplyMaterialUpdatesToObject(intensityByFactory * strength);
        }
    }
}
