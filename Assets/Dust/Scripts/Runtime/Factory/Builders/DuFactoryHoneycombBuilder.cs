using UnityEngine;

namespace DustEngine
{
    public class DuFactoryHoneycombBuilder : DuFactoryBuilder
    {
        public override void Initialize(DuFactory duFactory)
        {
            var honeycombFactory = duFactory as DuHoneycombFactory;

            base.Initialize(duFactory);

            DuRandom duRandom = new DuRandom(honeycombFactory.offsetSeed);

            bool isInvertDirection = honeycombFactory.offsetDirection == DuHoneycombFactory.OffsetDirection.Width;

            int buildLengthX = isInvertDirection ? honeycombFactory.height : honeycombFactory.width;
            int buildLengthY = isInvertDirection ? honeycombFactory.width : honeycombFactory.height;

            Vector2 stepOffset;
            stepOffset.x = honeycombFactory.sizeWidth;
            stepOffset.y = honeycombFactory.sizeHeight;

            Vector2 zeroPoint;
            zeroPoint.x = -(buildLengthX - 1) / 2f * stepOffset.x;
            zeroPoint.y = -(Mathf.CeilToInt(buildLengthY / 2f) - 1) / 2f * stepOffset.y;

            for (int x = 0; x < buildLengthX; x++)
            {
                float randOffsetX = duRandom.Next();
                float randOffsetY = duRandom.Next();

                bool isOddCol = x % 2 == 1;
                bool isApplyExtraOffsets = x > 0 && x < buildLengthX - 1;

                int halfLengthY = isOddCol ? Mathf.FloorToInt(buildLengthY / 2f) : Mathf.CeilToInt(buildLengthY / 2f);

                for (int y = 0; y < halfLengthY; y++)
                {
                    switch (honeycombFactory.honeycombForm)
                    {
                        case DuHoneycombFactory.HoneycombForm.Square:
                            break;

                        case DuHoneycombFactory.HoneycombForm.Circle:
                            float filterX = ((float) x / (buildLengthX - 1) - 0.5f) * 2f;
                            float filterY = ((float) y / (halfLengthY - 1) - 0.5f) * 2f;

                            if (new Vector2(filterX, filterY).magnitude > 1f)
                                continue;

                            break;
                    }

                    Vector2 curPos;
                    curPos.x = zeroPoint.x + x * stepOffset.x;
                    curPos.y = zeroPoint.y + y * stepOffset.y;

                    Vector2 curUvw;
                    curUvw.x = (float) x / buildLengthX;
                    curUvw.y = (float) y / halfLengthY;

                    if (isOddCol)
                        curPos.y += honeycombFactory.offset * stepOffset.y;

                    if (isApplyExtraOffsets)
                    {
                        curPos.x += honeycombFactory.offsetPerpendicular * DuMath.Map(0f, 1f, -1f, +1f, randOffsetX) * stepOffset.x;
                        curPos.y -= honeycombFactory.offsetVariation * honeycombFactory.offset * randOffsetY * stepOffset.y;

                        curUvw.y += 1f / halfLengthY * honeycombFactory.offset;
                    }

                    var instanceState = new DuFactoryInstance.State();

                    instanceState.position = Vector3.zero;
                    instanceState.rotation = Vector3.zero;
                    instanceState.scale = Vector3.one;
                    instanceState.uvw = Vector3.zero;
                    instanceState.uvw.x = isInvertDirection ? curUvw.y : curUvw.x;
                    instanceState.uvw.y = isInvertDirection ? curUvw.x : curUvw.y;

                    switch (honeycombFactory.orientation)
                    {
                        case DuFactory.Orientation.XY:
                            instanceState.position.x = isInvertDirection ? curPos.y : curPos.x;
                            instanceState.position.y = isInvertDirection ? curPos.x : curPos.y;
                            break;

                        case DuFactory.Orientation.ZY:
                            instanceState.position.z = isInvertDirection ? curPos.y : curPos.x;
                            instanceState.position.y = isInvertDirection ? curPos.x : curPos.y;
                            break;

                        case DuFactory.Orientation.XZ:
                            instanceState.position.x = isInvertDirection ? curPos.y : curPos.x;
                            instanceState.position.z = isInvertDirection ? curPos.x : curPos.y;
                            break;
                    }

                    m_InstancesStates.Add(instanceState);
                }
            }
        }
    }
}
