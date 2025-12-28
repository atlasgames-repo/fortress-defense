using System;
using UnityEngine;

namespace Assets.FantasyMonsters.Scripts.Utils
{
    /// <summary>
    /// Used to prepare textures for saving.
    /// </summary>
    public static class TextureHelper
    {
        public static Sprite CreateSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.one / 2, 100f, 0, SpriteMeshType.FullRect);
        }

        public static Texture2D MergeLayers(int width, int height, params Color[][] layers)
        {
            if (layers.Length == 0) throw new Exception("No layers to merge.");
            
            var result = new Color[width * height];

            foreach (var layer in layers)
            {
                if (layer == null) continue;

                for (var i = 0; i < result.Length; i++)
                {
                    var color = layer[i];

                    if (color.a < 1)
                    {
                        ToPremultipliedAlpha(ref color);
                        result[i] = result[i] * (1 - color.a) + color;
                    }
                    else
                    {
                        result[i] = color;
                    }
                }
            }

            for (var i = 0; i < result.Length; i++)
            {
                ToStraightAlpha(ref result[i]);
            }

            var texture = new Texture2D(width, height) { filterMode = FilterMode.Bilinear };

            texture.SetPixels(result);
            texture.Apply();

            return texture;
        }

        public static Rect GetContentRect(Texture2D texture)
        {
            var pixels = texture.GetPixels();
            var width = texture.width;
            var height = texture.height;
            var minX = width - 1;
            var minY = height - 1;
            var maxX = 0;
            var maxY = 0;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (pixels[x + y * width].a > 0)
                    {
                        minX = Mathf.Min(x, minX);
                        minY = Mathf.Min(y, minY);
                    }
                }
            }

            for (var x = width - 1; x >= 0; x--)
            {
                for (var y = height - 1; y >= 0; y--)
                {
                    if (pixels[x + y * width].a > 0)
                    {
                        maxX = Mathf.Max(x, maxX);
                        maxY = Mathf.Max(y, maxY);
                    }
                }
            }

            return new Rect(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }

        public static void Crop(Texture2D texture, Rect rect)
        {
            var pixels = texture.GetPixels((int) rect.xMin, (int) rect.yMin, (int) rect.height, (int) rect.width);

            texture.Reinitialize((int) rect.height, (int) rect.width);
            texture.SetPixels(pixels);
            texture.Apply();
        }

        public static Texture2D Center(Texture2D texture)
        {
            var rect = GetContentRect(texture);
            var pixels = texture.GetPixels((int) rect.min.x, (int) rect.min.y, (int) rect.width, (int) rect.height);
            var offsetX = (texture.width - rect.width) / 2;
            var offsetY = (texture.height - rect.height) / 2;

            texture.SetPixels(new Color[texture.width * texture.height]);
            texture.SetPixels((int) offsetX, (int) offsetY, (int) rect.width, (int) rect.height, pixels);
            texture.Apply();

            return texture;
        }

        public static Texture2D AddShadow(Texture2D texture, int offset, float opacity)
        {
            var pixels = texture.GetPixels();
            var result = new Color[pixels.Length];

            for (var y = 0; y < texture.height - offset; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var a = pixels[x + (y + offset) * texture.width].a;

                    result[x + y * texture.width] = new Color(0, 0, 0, a * opacity);
                }
            }

            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    result[x + y * texture.width] += pixels[x + y * texture.width];
                }
            }

            texture.SetPixels(result);
            texture.Apply();

            return texture;
        }

        public static Color AdjustColor(Color color, float hue, float saturation, float value)
        {
            hue /= 180f;
            saturation /= 100f;
            value /= 100f;

            var a = color.a;

            Color.RGBToHSV(color, out var h, out var s, out var v);

            h += hue / 2f;

            if (h > 1) h -= 1;
            else if (h < 0) h += 1;

            color = Color.HSVToRGB(h, s, v);

            var grey = 0.3f * color.r + 0.59f * color.g + 0.11f * color.b;

            color.r = grey + (color.r - grey) * (saturation + 1);
            color.g = grey + (color.g - grey) * (saturation + 1);
            color.b = grey + (color.b - grey) * (saturation + 1);

            if (color.r < 0) color.r = 0;
            if (color.g < 0) color.g = 0;
            if (color.b < 0) color.b = 0;

            color.r += value * color.r;
            color.g += value * color.g;
            color.b += value * color.b;
            color.a = a;

            return color;
        }

        private static void ToPremultipliedAlpha(ref Color color)
        {
            color.r *= color.a;
            color.g *= color.a;
            color.b *= color.a;
        }

        private static void ToStraightAlpha(ref Color color)
        {
            color.r /= color.a;
            color.g /= color.a;
            color.b /= color.a;
        }
    }
}