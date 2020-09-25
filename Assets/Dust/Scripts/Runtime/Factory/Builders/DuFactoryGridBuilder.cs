using UnityEngine;

namespace DustEngine
{
    public class DuFactoryGridBuilder : DuFactoryBuilder
    {
        public override void Initialize(DuFactory duFactory)
        {
            var gridFactory = duFactory as DuGridFactory;

            base.Initialize(duFactory);

            Vector3Int gridCount = Vector3Int.Max(Vector3Int.one, gridFactory.count);

            Vector3 stepOffset = gridFactory.size;

            Vector3 zeroPoint;
            zeroPoint.x = -(gridCount.x - 1) / 2f * stepOffset.x;
            zeroPoint.y = -(gridCount.y - 1) / 2f * stepOffset.y;
            zeroPoint.z = -(gridCount.z - 1) / 2f * stepOffset.z;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            for (int z = 0; z < gridFactory.count.z; z++)
            for (int y = 0; y < gridFactory.count.y; y++)
            for (int x = 0; x < gridFactory.count.x; x++)
            {
                var instanceState = new DuFactoryInstance.State();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Set position/rotation/scale

                instanceState.position = new Vector3(zeroPoint.x + stepOffset.x * x, zeroPoint.y + stepOffset.y * y, zeroPoint.z + stepOffset.z * z);
                instanceState.rotation = Vector3.zero;
                instanceState.scale = Vector3.one;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // UV

                float uvwX = gridCount.x > 1 ? 1f / (gridCount.x - 1) : 0f;
                float uvwY = gridCount.y > 1 ? 1f / (gridCount.y - 1) : 0f;
                float uvwZ = gridCount.z > 1 ? 1f / (gridCount.z - 1) : 0f;

                instanceState.uvw = new Vector3(x * uvwX, y * uvwY, z * uvwZ);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                m_InstancesStates.Add(instanceState);
            }
            // end of for:3x
        }
    }
}
