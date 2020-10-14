using UnityEngine;

namespace DustEngine
{
    //
    // Agreement:
    // All methods return values between 0..+1
    // Only Wide methods return values between -1..+1
    //
    public class DuNoise
    {
        private readonly float PerlinNoise_XX;
        private readonly float PerlinNoise_XY;
        private readonly float PerlinNoise_YX;
        private readonly float PerlinNoise_YY;
        private readonly float PerlinNoise_ZX;
        private readonly float PerlinNoise_ZY;

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
        // Perlin 1D noise
        // - input: float
        // - return: float

        public float Perlin1D(float v)
            => Perlin1D(v, 0f);

        public float Perlin1D(float v, float offset)
        {
            return Mathf.Clamp01(Mathf.PerlinNoise(PerlinNoise_XX + v, PerlinNoise_XY + offset));
        }

        public float Perlin1D_asWide(float v)
            => Perlin1D_asWide(v, 0f);

        public float Perlin1D_asWide(float v, float offset)
        {
            return Perlin1D(v, offset) * 2f - 1f;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 1D noise
        // - input: float
        // - return: Vector3

        public Vector3 Perlin1D_asVector3(float v)
            => Perlin1D_asVector3(v, 0f);

        public Vector3 Perlin1D_asVector3(float v, float offset)
        {
            Vector3 result;
            result.x = Mathf.Clamp01(Mathf.PerlinNoise(PerlinNoise_XX + v, PerlinNoise_XY + offset));
            result.y = Mathf.Clamp01(Mathf.PerlinNoise(PerlinNoise_YX + v, PerlinNoise_YY + offset));
            result.z = Mathf.Clamp01(Mathf.PerlinNoise(PerlinNoise_ZX + v, PerlinNoise_ZY + offset));
            return result;
        }

        public Vector3 Perlin1D_asWideVector3(float v)
            => Perlin1D_asWideVector3(v, 0f);

        public Vector3 Perlin1D_asWideVector3(float v, float offset)
        {
            return Perlin1D_asVector3(offset) * 2f - Vector3.one;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 2D noise
        // - input: Vector2
        // - return: float

        public float Perlin2D(Vector2 v)
            => Perlin2D(v.x, v.y, 0f);

        public float Perlin2D(Vector2 v, float offset)
            => Perlin2D(v.x, v.y, offset);

        public float Perlin2D(float x, float y)
            => Perlin2D(x, y, 0f);

        public float Perlin2D(float x, float y, float offset)
        {
            return Mathf.PerlinNoise(PerlinNoise_XX + x + offset, PerlinNoise_YY + y + offset);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 2D noise
        // - input: Vector2
        // - return: Vector2

        public Vector2 Perlin2D_asVector2(Vector2 v)
            => Perlin2D_asVector2(v.x, v.y, 0f);

        public Vector2 Perlin2D_asVector2(Vector2 v, float offset)
            => Perlin2D_asVector2(v.x, v.y, offset);

        public Vector2 Perlin2_asVector2D(float x, float y)
            => Perlin2D_asVector2(x, y, 0f);

        public Vector2 Perlin2D_asVector2(float x, float y, float offset)
        {
            Vector2 result;
            result.x = Perlin2D(x, y, +offset);
            result.y = Perlin2D(y, x, -offset);
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 3D noise
        // - input: Vector3
        // - return: float

        public float Perlin3D(Vector3 v)
            => Perlin3D(v.x, v.y, v.z, 0f);

        public float Perlin3D(Vector3 v, float offset)
            => Perlin3D(v.x, v.y, v.z, offset);

        public float Perlin3D(float x, float y, float z)
            => Perlin3D(x, y, z, 0f);

        public float Perlin3D(float x, float y, float z, float offset)
        {
            float x1 = Mathf.PerlinNoise(PerlinNoise_XX + x + offset, PerlinNoise_XY + y + offset);
            float x2 = Mathf.PerlinNoise(PerlinNoise_XX + x + offset, PerlinNoise_XY + z + offset);

            float y1 = Mathf.PerlinNoise(PerlinNoise_YX + y + offset, PerlinNoise_YY + x + offset);
            float y2 = Mathf.PerlinNoise(PerlinNoise_YX + y + offset, PerlinNoise_YY + z + offset);

            float z1 = Mathf.PerlinNoise(PerlinNoise_ZX + z + offset, PerlinNoise_ZY + x + offset);
            float z2 = Mathf.PerlinNoise(PerlinNoise_ZX + z + offset, PerlinNoise_ZY + y + offset);

            return Mathf.Clamp01((x1 + x2 + y1 + y2 + z1 + z2) / 6f);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Perlin 3D noise
        // - input: Vector3
        // - return: Vector3

        public Vector3 Perlin3D_asVector3(Vector3 v)
            => Perlin3D_asVector3(v.x, v.y, v.z, 0f);

        public Vector3 Perlin3D_asVector3(Vector3 v, float offset)
            => Perlin3D_asVector3(v.x, v.y, v.z, offset);

        public Vector3 Perlin3_asVector3D(float x, float y, float z)
            => Perlin3D_asVector3(x, y, z, 0f);

        public Vector3 Perlin3D_asVector3(float x, float y, float z, float offset)
        {
            Vector3 result;
            result.x = Perlin3D(x, y, z, offset);
            result.y = Perlin3D(y, z, x, offset);
            result.z = Perlin3D(z, x, y, offset);
            return result;
        }
    }
}
