using UnityEngine;

namespace DustEngine
{
    public static partial class DuMath
    {
        public static bool IsZero(float value)
        {
            return Mathf.Approximately(value, 0f);
        }

        public static bool IsNotZero(float value)
        {
            return !IsZero(value);
        }

        public static bool Between(float value, float a, float b)
        {
            return a <= value && value <= b;
        }

        public static float Round(float value, int digits = DuConstants.ROUND_DIGITS_COUNT)
        {
            if (digits == 0)
                return value;

            return (float) System.Math.Round(value, digits);
        }

        public static float Length(float a, float b)
        {
            return Mathf.Sqrt(a * a + b * b);
        }

        public static float NormalizeAngle180(float value)
        {
            value -= FloorToZero(value / 360f) * 360f;
            while (value > +180f) value -= 360f;
            while (value < -180f) value += 360f;
            return value;
        }

        public static float NormalizeAngle360(float value)
        {
            value -= FloorToZero(value / 360f) * 360f;
            while (value > +360f) value -= 360f;
            while (value <    0f) value += 360f;
            return value;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static float FloorToZero(float value)
        {
            return value >= 0f ? Mathf.Floor(value) : Mathf.Ceil(value);
        }

        public static int FloorToZeroToInt(float value)
        {
            return value >= 0f ? Mathf.FloorToInt(value) : Mathf.CeilToInt(value);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static float Repeat(float t, float length)
        {
            // Why need this?
            // Because for example if length = 1f, then Repeat(1f, 1f) will be 0f, but should be 1f
            if (t < 0f || t > length)
                t = Mathf.Repeat(t, length);

            return t;
        }

        public static float Step01(float value, int stepsCount)
        {
            if (stepsCount < 1)
                return 0.0f;

            float stepDelta = 1f / stepsCount;
            float stepIndex = value / stepDelta;

            return Mathf.RoundToInt(stepIndex) * stepDelta;
        }

        public static float Step(float value, int stepsCount, float min, float max)
        {
            if (stepsCount < 1)
                return 0.0f;

            float valueNormalized = Map(min, max, 0f, 1f, value);

            valueNormalized = Step01(valueNormalized, stepsCount);

            return Map01To(min, max, valueNormalized);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static float Map(float inMin, float inMax, float outMin, float outMax, float inValue, bool clamped = false)
        {
            if (clamped)
            {
                if (inValue <= inMin) return outMin;
                if (inValue >= inMax) return outMax;
            }

            float inRange = inMax - inMin;
            float outRange = outMax - outMin;

            if (IsZero(inRange))
                return outMin + outRange / 2f; // just to prevent divide by ZERO -> return middle value from [min..max]-out

            return outMin + (inValue - inMin) / inRange * outRange;
        }

        public static float Map01To(float outMin, float outMax, float inValue, bool clamped = false)
        {
            if (clamped)
            {
                if (inValue <= 0f) return outMin;
                if (inValue >= 1f) return outMax;
            }

            return outMin + (outMax - outMin) * inValue;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void RotatePoint(ref float x, ref float y, float angle)
        {
            float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
            float cos = Mathf.Cos(Mathf.Deg2Rad * angle);

            float xNew = x * cos - y * sin;
            float yNew = x * sin + y * cos;

            x = xNew;
            y = yNew;
        }

        public static Vector2 RotatePoint(float x, float y, float angle)
        {
            RotatePoint(ref x, ref y, angle);
            return new Vector2(x, y);
        }

        public static Vector2 RotatePoint(Vector2 point, float angle)
        {
            return RotatePoint(point.x, point.y, angle);
        }

        /*
        public static Vector2 RotatePointAroundPoint(Vector2 point, float angle, Vector2 center)
        {
            float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
            float cos = Mathf.Cos(Mathf.Deg2Rad * angle);

            // translate point back to origin:
            point.x -= center.x;
            point.y -= center.y;

            // rotate point + translate point back
            float xNew = (point.x * cos - point.y * sin);
            float yNew = (point.x * sin + point.y * cos);

            // translate point back:
            point.x = xNew + center.x;
            point.y = yNew + center.y;
            return point;
        }
        */

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // @DUST.optimize: make it better!
        public static Vector3 RotatePoint(Vector3 pointToRotate, Vector3 eulerRotation)
        {
            if (eulerRotation.Equals(Vector3.zero))
                return pointToRotate;

            Quaternion q = Quaternion.Euler(eulerRotation);
            Matrix4x4 m4r = Matrix4x4.Rotate(q);
            return m4r.MultiplyPoint(pointToRotate);
        }
    }
}
