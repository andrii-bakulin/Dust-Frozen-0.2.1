using UnityEngine;

namespace DustEngine
{
    public class DuNoise
    {
        readonly float PerlinNoise_XX;
        readonly float PerlinNoise_XY;
        readonly float PerlinNoise_YX;
        readonly float PerlinNoise_YY;
        readonly float PerlinNoise_ZX;
        readonly float PerlinNoise_ZY;

        public DuNoise(int seed)
        {
            if (seed <= 0)
                seed = Random.Range(1, int.MaxValue);

            DuRandom duRandom = new DuRandom(seed);

            PerlinNoise_XX = duRandom.Range(-9999f, +9999f);
            PerlinNoise_XY = duRandom.Range(-9999f, +9999f);

            PerlinNoise_YX = duRandom.Range(-9999f, +9999f);
            PerlinNoise_YY = duRandom.Range(-9999f, +9999f);

            PerlinNoise_ZX = duRandom.Range(-9999f, +9999f);
            PerlinNoise_ZY = duRandom.Range(-9999f, +9999f);
        }

        //--------------------------------------------------------------------------------------------------------------

        /**
         * return: value range[0..1]
         */
        public float PerlinNoise(float offset)
        {
            return Mathf.Clamp01(Mathf.PerlinNoise(PerlinNoise_XX + offset, PerlinNoise_XY + offset));
        }

        /**
         * return: value range[-1..1]
         */
        public float PerlinNoiseWide(float offset)
        {
            return PerlinNoise(offset) * 2f - 1f;
        }

        public float PerlinNoise(Vector3 v)
        {
            return PerlinNoise(v.x, v.y, v.z);
        }

        public float PerlinNoise(float x, float y, float z)
        {
            float x1 = Mathf.PerlinNoise(PerlinNoise_XX + x, PerlinNoise_XY + y);
            float x2 = Mathf.PerlinNoise(PerlinNoise_XX + x, PerlinNoise_XY + z);

            float y1 = Mathf.PerlinNoise(PerlinNoise_YX + y, PerlinNoise_YY + x);
            float y2 = Mathf.PerlinNoise(PerlinNoise_YX + y, PerlinNoise_YY + z);

            float z1 = Mathf.PerlinNoise(PerlinNoise_ZX + z, PerlinNoise_ZY + x);
            float z2 = Mathf.PerlinNoise(PerlinNoise_ZX + z, PerlinNoise_ZY + y);

            return Mathf.Clamp01((x1 + x2 + y1 + y2 + z1 + z2) / 6f);
        }

        /**
         * return: each component value between range[0..1]
         */
        public Vector3 PerlinNoiseVector3()
            => PerlinNoiseVector3(0f);

        public Vector3 PerlinNoiseVector3(float offset)
        {
            float x = Mathf.Clamp01(Mathf.PerlinNoise(PerlinNoise_XX + offset, PerlinNoise_XY + offset));
            float y = Mathf.Clamp01(Mathf.PerlinNoise(PerlinNoise_YX + offset, PerlinNoise_YY + offset));
            float z = Mathf.Clamp01(Mathf.PerlinNoise(PerlinNoise_ZX + offset, PerlinNoise_ZY + offset));
            return new Vector3(x, y, z);
        }

        /**
         * return: each component value between range[0..1]
         */
        public Vector3 PerlinNoiseVector3(Vector3 position)
            => PerlinNoiseVector3(position, 0f);

        public Vector3 PerlinNoiseVector3(Vector3 position, float offset)
        {
            float x = PerlinNoise(position.x + offset, position.y + offset, position.z + offset);
            float y = PerlinNoise(position.y + offset, position.z + offset, position.x + offset);
            float z = PerlinNoise(position.z + offset, position.x + offset, position.y + offset);
            return new Vector3(x, y, z);
        }

        /**
         * return: each component value between range[-1..1]
         */
        public Vector3 PerlinNoiseWideVector3(float offset)
        {
            return PerlinNoiseVector3(offset) * 2f - Vector3.one;
        }
    }
}
