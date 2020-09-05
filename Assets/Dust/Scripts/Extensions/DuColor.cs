using UnityEngine;

namespace DustEngine
{
    public static class DuColor
    {
        //--------------------------------------------------------------------------------------------------------------
        // Extensions

        public static void Clamp(ref this Color self, float min, float max)
        {
            self.r = Mathf.Clamp(self.r, min, max);
            self.g = Mathf.Clamp(self.g, min, max);
            self.b = Mathf.Clamp(self.b, min, max);
            self.a = Mathf.Clamp(self.a, min, max);
        }

        public static void Clamp01(ref this Color self)
        {
            self.Clamp(0f, 1f);
        }

        public static void InvertRGB(ref this Color self)
        {
            self.r = 1f - self.r;
            self.g = 1f - self.g;
            self.b = 1f - self.b;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Color ToRGBWithoutAlpha(this Color self)
        {
            self.r *= self.a;
            self.g *= self.a;
            self.b *= self.a;
            self.a = 1f;
            return self;
        }

        public static Vector3 ToVector3(this Color self)
            => ToVector3(self, 0);

        public static Vector3 ToVector3(this Color self, int digits)
        {
            var v = new Vector3();
            v.x = DuMath.Round(self.r, digits);
            v.y = DuMath.Round(self.g, digits);
            v.z = DuMath.Round(self.b, digits);
            return v;
        }

        public static Vector3Int ToVector3Int(this Color self)
        {
            var v = new Vector3Int();
            v.x = Mathf.RoundToInt(self.r);
            v.y = Mathf.RoundToInt(self.g);
            v.z = Mathf.RoundToInt(self.b);
            return v;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Creators

        public static Color RandomColor()
        {
            return Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f);
        }

        public static Color Min(Color a, Color b)
        {
            Color r;
            r.r = Mathf.Min(a.r, b.r);
            r.g = Mathf.Min(a.g, b.g);
            r.b = Mathf.Min(a.b, b.b);
            r.a = Mathf.Min(a.a, b.a);
            return r;
        }

        public static Color Max(Color a, Color b)
        {
            Color r;
            r.r = Mathf.Max(a.r, b.r);
            r.g = Mathf.Max(a.g, b.g);
            r.b = Mathf.Max(a.b, b.b);
            r.a = Mathf.Max(a.a, b.a);
            return r;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Color InvertRGB(Color color)
        {
            color.InvertRGB();
            return color;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static Color AddRGB(Color a, Color b)
            => AddRGB(a, b, 1f);

        public static Color AddRGB(Color a, Color b, float alpha)
        {
            a.r = a.r + b.r;
            a.g = a.g + b.g;
            a.b = a.b + b.b;
            a.a = alpha;
            a.Clamp01();
            return a;
        }

        public static Color SubtractRGB(Color a, Color b)
            => SubtractRGB(a, b, 1f);

        public static Color SubtractRGB(Color a, Color b, float alpha)
        {
            a.r = a.r - b.r;
            a.g = a.g - b.g;
            a.b = a.b - b.b;
            a.a = alpha;
            a.Clamp01();
            return a;
        }

        public static Color DivideSafely(Color a, Color b)
        {
            if (b.r > 0f) a.r /= b.r;
            if (b.g > 0f) a.g /= b.g;
            if (b.b > 0f) a.b /= b.b;
            if (b.a > 0f) a.a /= b.a;
            return a;
        }
    }
}
