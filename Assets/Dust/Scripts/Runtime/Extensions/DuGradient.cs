using UnityEngine;

namespace DustEngine
{
    public static class DuGradient
    {
        public static Gradient CreateBlackToRed()
            => CreateBlackToColor(Color.red);

        public static Gradient CreateBlackToColor(Color color)
        {
            var gradient = new Gradient();

            gradient.SetKeys(
                new[] {new GradientColorKey(Color.black, 0f), new GradientColorKey(color, 1f)},
                new[] {new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f)}
            );

            return gradient;
        }
    }
}
