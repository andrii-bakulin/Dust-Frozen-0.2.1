using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace DustEngine.DustEditor
{
    public static class DuConstants
    {
        internal const int RANDOM_SEED_DEFAULT = 12345;
        internal const int RANDOM_SEED_MIN = 1;
        internal const int RANDOM_SEED_MAX = 99999;
    }
}
#endif
