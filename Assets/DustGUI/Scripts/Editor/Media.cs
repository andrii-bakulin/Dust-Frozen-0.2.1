using UnityEngine;
using UnityEditor;

namespace DustEngine
{
    public static partial class DustGUI
    {
        public static void Image(Rect rect, Texture texture)
        {
            EditorGUI.LabelField(rect, new GUIContent(texture));
        }

        public static void Gradient(Rect rect, Gradient gradient)
        {
            EditorGUI.LabelField(rect, new GUIContent(CreateGradientTexture(gradient, rect.width, rect.height)));
        }

        //--------------------------------------------------------------------------------------------------------------

        public static Texture2D CreateGradientTexture(Gradient gradient, float width, float height)
            => CreateGradientTexture(gradient, (int) width, (int) height);

        public static Texture2D CreateGradientTexture(Gradient gradient, int width, int height)
        {
            if (width < 0)
                width = 8;

            if (height < 0)
                height = 8;

            var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            texture.hideFlags = HideFlags.HideAndDontSave;

            Color[] pixels = new Color[width * height];

            for (int x = 0; x < width; x++)
            {
                Color color = gradient.Evaluate((float) x / (width - 1));

                for (int y = 0; y < height; y++)
                {
                    pixels[y * width + x] = color;
                }
            }

            texture.SetPixels(pixels);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();

            return texture;
        }
    }
}
