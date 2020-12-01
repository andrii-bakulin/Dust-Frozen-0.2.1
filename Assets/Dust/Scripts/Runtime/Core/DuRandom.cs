using UnityEngine;

namespace DustEngine
{
    public class DuRandom
    {
        readonly System.Random random;

        public DuRandom(int seed)
        {
            if (seed <= 0)
                seed = Random.Range(1, int.MaxValue);

            random = new System.Random(seed);
        }

        /**
         * This normalization prevent usage of negative values
         */
        public static int NormalizeSeedToNonRandom(int seed)
        {
            return Mathf.Abs(seed);
        }

        /**
         * return: (0..1)
         */
        public float Next()
        {
            return (float) random.NextDouble();
        }

        public Vector3 NextVector3()
        {
            return new Vector3(Next(), Next(), Next());
        }

        /**
         * return: (min, max)
         */
        public float Range(float min, float max)
        {
            return min + (max - min) * Next();
        }

        /**
         * return: [min, max)
         */
        public int Range(int min, int max)
        {
            return random.Next(min, max);
        }

        public Vector3 Range(Vector3 min, Vector3 max)
        {
            return new Vector3(Range(min.x, max.x), Range(min.y, max.y), Range(min.z, max.z));
        }
    }
}
