using System;
using UnityEngine;

namespace DustEngine
{
    [Serializable]
    public struct DuRange : IEquatable<DuRange>
    {
        public float min { get; set; }
        public float max { get; set; }

        private static readonly DuRange zeroToOneRange = new DuRange(0.0f, 1.0f);
        private static readonly DuRange oneToTwoRange = new DuRange(1.0f, 2.0f);

        public static DuRange zeroToOne => zeroToOneRange;
        public static DuRange oneToTwo => oneToTwoRange;

        public DuRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Equals(DuRange other)
        {
            return Mathf.Approximately(this.min, other.min) && Mathf.Approximately(this.max, other.max);
        }
    }
}
