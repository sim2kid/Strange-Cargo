using System;
using System.Collections;
using UnityEngine;

namespace Utility
{
    public static class TextureConversions
    {
        public const int PIXELS_PER_FRAME = 5000;

        /// <summary>
        /// Converts a RGB image into a new color space provided by <paramref name="newColors"/>.
        /// This will be applied to the <paramref name="toModify"/> image and will add the greyscale image of <paramref name="textureDetails"/> on top.
        /// This process is non destructive of <paramref name="toModify"/>
        /// You can get the Texture2D in the <paramref name="callback"/>
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="toModify"></param>
        /// <param name="newColors"></param>
        /// <param name="textureDetails"></param>
        /// <returns></returns>
        public static IEnumerator ConvertTexture(Action<Texture2D> callback, Texture2D toModify, Color[] newColors, Texture2D textureDetails = null, int pixelsPerUpdate = 0) 
        {
            // Warning if the modify and details texture are different sizes. Cropping will happen since this is a pixel perfect operation.
            CheckDetailTexture(textureDetails, toModify);

            // The new texture's output so we aren't overriding a texture.
            Texture2D output = new Texture2D(toModify.width, toModify.height);

            // Loop through each pixel and set it's new color based on the old ones.
            for (int width = 0; width < output.width; width++)
            {
                for (int height = 0; height < output.height; height++)
                {
                    Vector4 patternColor = toModify.GetPixel(width, height);
                    //Only get this color if the texture isn't null or out of bounds
                    Vector4 detailColor = (textureDetails != null && textureDetails.width > width && textureDetails.height > height ?
                        textureDetails.GetPixel(width, height) : Color.clear);

                    output.SetPixel(width, height, forwardConvert(patternColor, detailColor, newColors));

                    if ((width * output.height + height) % (pixelsPerUpdate <= 0 ? PIXELS_PER_FRAME : pixelsPerUpdate) == 0) 
                    {
                        // Pause our conversion until next frame
                        yield return null;
                    }
                }
            }
            // Apply our changes!
            output.Apply();
            // Send out output through the callback!
            callback.Invoke(output);
        }

        /// <summary>
        /// Converts a texture to a representation of 4 values provided in <paramref name="baseColors"/>.
        /// You can get the Texture2D in the <paramref name="callback"/>
        /// The imange being converted is <paramref name="toModify"/>.
        /// This process is non destructive of <paramref name="toModify"/>
        /// NOTE: This process is not perfect and will get muddy with mixes of colors!!
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="toModify"></param>
        /// <param name="baseColors"></param>
        /// <returns></returns>
        public static IEnumerator GenerateBaseTexture(Action<Texture2D> callback, Texture2D toModify, Color[] baseColors, int pixelsPerUpdate = 0)
        {
            // The new texture's output so we aren't overriding a texture.
            Texture2D output = new Texture2D(toModify.width, toModify.height);

            // Loop through each pixel and set it's new color based on the old ones.
            for (int width = 0; width < output.width; width++)
            {
                for (int height = 0; height < output.height; height++)
                {
                    Vector4 patternColor = toModify.GetPixel(width, height);
                    //Only get this color if the texture isn't null or out of bounds

                    output.SetPixel(width, height, backwardsConvert(patternColor, baseColors));

                    if ((width * output.height + height) % (pixelsPerUpdate <= 0 ? PIXELS_PER_FRAME : pixelsPerUpdate) == 0)
                    {
                        // Pause our conversion until next frame
                        yield return null;
                    }
                }
            }
            // Apply our changes!
            output.Apply();
            // Send out output through the callback!
            callback.Invoke(output);
        }

        private static Vector4 forwardConvert(Vector4 patternColor, Vector4 detailColor, Color[] newColors) 
        {
            // We multiply each channel with the associated color
            Vector4 r = (newColors.Length >= 1 ? newColors[0] : Color.clear) * patternColor.x;
            Vector4 g = (newColors.Length >= 2 ? newColors[1] : Color.clear) * patternColor.y;
            Vector4 b = (newColors.Length >= 3 ? newColors[2] : Color.clear) * patternColor.z;
            Vector4 a = (newColors.Length >= 4 ? newColors[3] : Color.clear) * patternColor.w;

            // We add those colors together, then we apply the coat texture over that.
            Vector4 outColor = (r + g + b + a) * (1 - (((detailColor.x + detailColor.y + detailColor.z) / 3) * detailColor.w));
            return outColor;
        }

        private static Vector4 backwardsConvert(Vector4 patternColor, Color[] baseColors) 
        {
            // We multiply each channel with the associated color
            Vector2 r = GetComponentValue(baseColors.Length >= 1 ? baseColors[0] : Color.clear, patternColor);
            Vector2 g = GetComponentValue(baseColors.Length >= 2 ? baseColors[1] : Color.clear, patternColor);
            Vector2 b = GetComponentValue(baseColors.Length >= 3 ? baseColors[2] : Color.clear, patternColor);
            Vector2 a = GetComponentValue(baseColors.Length >= 4 ? baseColors[3] : Color.clear, patternColor);

            float r1 = ApproximateColor(float.IsNaN(r.y) ? 1 : (r.y));
            float g1 = ApproximateColor(float.IsNaN(g.y) ? 1 : (g.y));
            float b1 = ApproximateColor(float.IsNaN(b.y) ? 1 : (b.y));
            float a1 = ApproximateColor(float.IsNaN(a.y) ? 1 : (a.y));

            Vector4 n = new Vector4(r1, g1, b1, a1);
            n.Normalize();

            float t = n.x + n.y + n.z + n.w;
            // We add those colors together, then we apply the coat texture over that.
            Vector4 outColor = new Vector4((float.IsInfinity(r.x) ? 1 : r.x) * (n.x / t),
                (float.IsInfinity(g.x) ? 1 : g.x) * (n.y / t),
                (float.IsInfinity(b.x) ? 1 : b.x) * (n.z / t),
                (float.IsInfinity(a.x) ? 1 : a.x) * (n.w / t));

            //TECHNICAL DEBT - Set transparency to 1;
            outColor.w = 1;

            return outColor;
        }

        private static float ApproximateColor(float x) 
        {
            return -Mathf.Log(x + 0.002f,500) + 0f;
        }

        private static Vector2 GetComponentValue(Vector4 baseColor, Vector4 actualColor) 
        {
            float distanceToBlack = Vector4.Distance(baseColor, Vector4.zero);
            float distanceToActual = Vector4.Distance(baseColor, actualColor);
            float actualAltered = distanceToBlack - distanceToActual;
            float distanceRatio = distanceToActual / distanceToBlack;

            Vector4 pointOnLine = LinePoint(distanceRatio, baseColor);
            float distanceToPoint = Vector4.Distance(pointOnLine, actualColor);

            return new Vector2((actualAltered / distanceToBlack), distanceToPoint);
        }

        private static Vector4 LinePoint(float ratio, Vector4 endpoint) 
        {
            float x = PointFinder(ratio, endpoint.x, 0);
            float y = PointFinder(ratio, endpoint.y, 0);
            float z = PointFinder(ratio, endpoint.z, 0);
            float w = PointFinder(ratio, endpoint.w, 0);

            return new Vector4(x, y, z, w);
        }

        private static float PointFinder(float ratio, float start, float end) 
        {
            return ((1 - ratio) * start + (ratio * end));
        } 

        // Redundant Code Breakdown

        private static void CheckDetailTexture(Texture2D textureDetails, Texture2D toModify) 
        {
            // Warning if the modify and details texture are different sizes. Cropping will happen since this is a pixel perfect operation.
            if (textureDetails != null)
            {
                if (toModify.width != textureDetails.width || toModify.height != textureDetails.height)
                {
                    Debug.LogWarning($"{textureDetails.name} is not the same size as {toModify.name}! " +
                        $"The images will not be rescaled to compensate for this! {toModify.name}'s size is {toModify.width}px by {toModify.height}px.");
                }
            }
        }
    }
}