using UnityEngine;

namespace DustEngine
{
    public static class DuVector3
    {
        //--------------------------------------------------------------------------------------------------------------
        // Extensions for Vector3

        public static void InverseScale(ref this Vector3 self, Vector3 scale)
        {
            self.x /= scale.x;
            self.y /= scale.y;
            self.z /= scale.z;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static void Abs(ref this Vector3 self)
        {
            self = DuVector3.Abs(self);
        }

        public static Vector3 abs(this Vector3 self) => DuVector3.Abs(self);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static void Map01To(ref this Vector3 self, float min, float max, bool clamped = false)
        {
            self.x = DuMath.Map01To(min, max, self.x, clamped);
            self.y = DuMath.Map01To(min, max, self.y, clamped);
            self.z = DuMath.Map01To(min, max, self.z, clamped);
        }

        public static void Map01To(ref this Vector3 self, Vector3 min, Vector3 max, bool clamped = false)
        {
            self.x = DuMath.Map01To(min.x, max.x, self.x, clamped);
            self.y = DuMath.Map01To(min.y, max.y, self.y, clamped);
            self.z = DuMath.Map01To(min.z, max.z, self.z, clamped);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Color ToColor(this Vector3 self, float alpha = 1.0f)
        {
            return new Color(self.x, self.y, self.z, alpha);
        }

        public static Vector3 ToRound(this Vector3 self, int digits)
        {
            self.x = DuMath.Round(self.x, digits);
            self.y = DuMath.Round(self.y, digits);
            self.z = DuMath.Round(self.z, digits);
            return self;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Helpers

        public static Vector3 New(float xyz)
        {
            return new Vector3(xyz, xyz, xyz);
        }

        public static Vector3 Abs(Vector3 value)
        {
            value.x = Mathf.Abs(value.x);
            value.y = Mathf.Abs(value.y);
            value.z = Mathf.Abs(value.z);
            return value;
        }

        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            value.z = Mathf.Clamp(value.z, min.z, max.z);
            return value;
        }

        public static Vector3 Clamp01(Vector3 value)
        {
            return Clamp(value, Vector3.zero, Vector3.one);
        }

        public static Vector3 NormalizeAngle180(Vector3 value)
        {
            value.x = DuMath.NormalizeAngle180(value.x);
            value.y = DuMath.NormalizeAngle180(value.y);
            value.z = DuMath.NormalizeAngle180(value.z);
            return value;
        }

        public static Vector3 NormalizeAngle360(Vector3 value)
        {
            value.x = DuMath.NormalizeAngle360(value.x);
            value.y = DuMath.NormalizeAngle360(value.y);
            value.z = DuMath.NormalizeAngle360(value.z);
            return value;
        }

        public static Vector3 Round(Vector3 value, int digits = DuConstants.ROUND_DIGITS_COUNT)
        {
            value.x = DuMath.Round(value.x, digits);
            value.y = DuMath.Round(value.y, digits);
            value.z = DuMath.Round(value.z, digits);
            return value;
        }

        public static Vector3 Map(float inMin, float inMax, Vector3 outMin, Vector3 outMax, float inValue, bool clamped = false)
        {
            Vector3 r;
            r.x = DuMath.Map(inMin, inMax, outMin.x, outMax.x, inValue, clamped);
            r.y = DuMath.Map(inMin, inMax, outMin.y, outMax.y, inValue, clamped);
            r.z = DuMath.Map(inMin, inMax, outMin.z, outMax.z, inValue, clamped);
            return r;
        }

        public static Vector3 Map01To(Vector3 outMin, Vector3 outMax, float inValue, bool clamped = false)
        {
            Vector3 r;
            r.x = DuMath.Map01To(outMin.x, outMax.x, inValue, clamped);
            r.y = DuMath.Map01To(outMin.y, outMax.y, inValue, clamped);
            r.z = DuMath.Map01To(outMin.z, outMax.z, inValue, clamped);
            return r;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = Mathf.Infinity)
        {
            Vector3 r;
            r.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed);
            r.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed);
            r.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed);
            return r;
        }

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        {
            Vector3 r;
            r.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime);
            r.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime);
            r.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime);
            return r;
        }

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, Vector3 smoothTime, float maxSpeed = Mathf.Infinity)
        {
            Vector3 r;
            r.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime.x, maxSpeed);
            r.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime.y, maxSpeed);
            r.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime.z, maxSpeed);
            return r;
        }

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, Vector3 smoothTime, float maxSpeed, float deltaTime)
        {
            Vector3 r;
            r.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime.x, maxSpeed, deltaTime);
            r.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime.y, maxSpeed, deltaTime);
            r.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime.z, maxSpeed, deltaTime);
            return r;
        }
    }
}
