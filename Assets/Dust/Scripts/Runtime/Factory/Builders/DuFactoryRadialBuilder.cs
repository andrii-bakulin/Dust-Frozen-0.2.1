using UnityEngine;

namespace DustEngine
{
    public class DuFactoryRadialBuilder : DuFactoryBuilder
    {
        public override void Initialize(DuFactory duFactory)
        {
            var radialFactory = duFactory as DuRadialFactory;

            base.Initialize(duFactory);

            DuRandom duRandom = new DuRandom(radialFactory.offsetSeed);

            for (int levelIndex = 0; levelIndex < radialFactory.levelsCount; levelIndex++)
            {
                int instancesCountLevelN = Mathf.Max(0, radialFactory.count + radialFactory.deltaCountPerLevel * levelIndex);
                float radiusThisLevel = radialFactory.radius + radialFactory.levelRadiusOffset * levelIndex;

                for (int instanceIndex = 0; instanceIndex < instancesCountLevelN; instanceIndex++)
                {
                    float angle = Mathf.Lerp(radialFactory.startAngle, radialFactory.endAngle, (float) instanceIndex / instancesCountLevelN);

                    angle += radialFactory.offset * (1f + radialFactory.offsetVariation * duRandom.Next());

                    Vector2 pos = new Vector2(Mathf.Sin(DuConstants.PI2 * angle / 360f), Mathf.Cos(DuConstants.PI2 * angle / 360f));

                    var instanceState = new DuFactoryInstance.State();

                    switch (radialFactory.orientation)
                    {
                        case DuFactory.Orientation.XY:
                            instanceState.position = new Vector3(pos.x, pos.y, 0f) * radiusThisLevel;
                            instanceState.rotation = radialFactory.align ? new Vector3(0f, 0f, -angle) : Vector3.zero;
                            break;

                        case DuFactory.Orientation.ZY:
                            instanceState.position = new Vector3(0f, pos.y, pos.x) * radiusThisLevel;
                            instanceState.rotation = radialFactory.align ? new Vector3(angle, 0f, 0f) : Vector3.zero;
                            break;

                        default:
                        case DuFactory.Orientation.XZ:
                            instanceState.position = new Vector3(pos.x, 0f, pos.y) * radiusThisLevel;
                            instanceState.rotation = radialFactory.align ? new Vector3(0f, angle, 0f) : Vector3.zero;
                            break;
                    }

                    instanceState.scale = Vector3.one;

                    if (instancesCountLevelN > 1)
                        instanceState.uvw = Vector3.Lerp(Vector3.zero, Vector3.right, 1f / (instancesCountLevelN - 1) * instanceIndex);
                    else
                        instanceState.uvw = Vector3.zero;

                    m_InstancesStates.Add(instanceState);
                }
            }
        }
    }
}
