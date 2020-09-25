using UnityEngine;

namespace DustEngine
{
    public partial class DuFactory
    {
        private void Update()
        {
            UpdateInstancesDynamicStates();
        }

        public void UpdateInstancesDynamicStates(bool forced = false)
        {
            if (!forced)
            {
                // @DUST.todo: make optimization here!
                // If factory-machines/fields/etc... didn't change states, then no need to update.

                if (factoryMachines.Count == 0)
                    return;
            }

            //----------------------------------------------------------------------------------------------------------
            // Step 1: reset instanceState to ZERO-State

            foreach (var factoryInstance in instances)
            {
                if (Dust.IsNull(factoryInstance))
                    continue;

                factoryInstance.ResetDynamicStateToZeroState();
            }

            //----------------------------------------------------------------------------------------------------------
            // Step 2: Calculate new transforms by Machines

            foreach (DuFactoryMachine.Record record in factoryMachines)
            {
                if (Dust.IsNull(record) || !record.enabled || Dust.IsNull(record.factoryMachine))
                    continue;

                if (DuMath.IsZero(record.intensity))
                    continue;

                if (!record.factoryMachine.gameObject.activeInHierarchy)
                    continue;

                record.factoryMachine.PrepareForUpdateInstancesStates(this);

                foreach (var instance in instances)
                {
                    if (Dust.IsNull(instance))
                        continue;

                    record.factoryMachine.UpdateInstanceState(this, instance, record.intensity);
                }

                record.factoryMachine.FinalizeUpdateInstancesStates(this);
            }

            //----------------------------------------------------------------------------------------------------------
            // Step 3: Apply Dynamic state to transform objects in scenes

            foreach (var factoryInstance in instances)
            {
                if (Dust.IsNull(factoryInstance))
                    continue;

                factoryInstance.ApplyDynamicStateToObject();
            }
        }
    }
}
