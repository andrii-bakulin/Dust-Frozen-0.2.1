using UnityEngine;

namespace DustEngine
{
    public static class DuConstants
    {
        public static readonly float PI = Mathf.PI;
        public static readonly float PI2 = Mathf.PI * 2f;
        public static readonly float EDITOR_UPDATE_TIMEOUT = 0.0166f;

        public static readonly int ROUND_DIGITS_COUNT = 6;

        public static readonly int RANDOM_SEED_DEFAULT = 12345;
        public static readonly int RANDOM_SEED_MIN = 1;
        public static readonly int RANDOM_SEED_MAX = 99999;
    }
}
