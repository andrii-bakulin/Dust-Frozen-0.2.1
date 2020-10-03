using UnityEngine;

namespace DustEngine
{
    public static class DuColor
    {
        //--------------------------------------------------------------------------------------------------------------
        // Extensions

        public static void Clamp(ref this Color self, float min, float max)
        {
            self = Clamp(self, min, max);
        }

        public static void Clamp(ref this Color self, Color min, Color max)
        {
            self = Clamp(self, min, max);
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

        public static Gradient ToGradient(this Color self)
        {
            var gradient = new Gradient();

            gradient.SetKeys(
                new[] {new GradientColorKey(self, 0f)},
                new[] {new GradientAlphaKey(1f, 0f)});

            return gradient;
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

        public static Color Clamp(Color color, float min, float max)
        {
            color.r = Mathf.Clamp(color.r, min, max);
            color.g = Mathf.Clamp(color.g, min, max);
            color.b = Mathf.Clamp(color.b, min, max);
            color.a = Mathf.Clamp(color.a, min, max);
            return color;
        }

        public static Color Clamp(Color color, Color min, Color max)
        {
            color.r = Mathf.Clamp(color.r, min.r, max.r);
            color.g = Mathf.Clamp(color.g, min.g, max.g);
            color.b = Mathf.Clamp(color.b, min.b, max.b);
            color.a = Mathf.Clamp(color.a, min.a, max.a);
            return color;
        }
    }
}
