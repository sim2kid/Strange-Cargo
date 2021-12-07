using System;
using System.Collections;
using UnityEngine;

namespace TextureConverter
{
    public static class TextureConversions
    {
        public const int PIXELS_PER_FRAME = 1000;

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
        public static IEnumerator ConvertTexture(Action<Texture2D> callback, Texture2D toModify, Color[] newColors, Texture2D textureDetails = null, int pixelsPerUpdate = 0, Action<float> ProgressReport = null) 
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
                        // Report progress
                        if (ProgressReport != null)
                            ProgressReport.Invoke((float)(width * output.height + height) / (output.width * output.height));
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
        public static IEnumerator GenerateBaseTexture(Action<Texture2D> callback, Texture2D toModify, Color[] baseColors, int pixelsPerUpdate = 0, Action<float> ProgressReport = null)
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
                        // Report progress
                        if (ProgressReport != null)
                            ProgressReport.Invoke((float)(width * output.height + height) / (output.width * output.height));
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
        /// Converts a RGB image into a new color space provided by <paramref name="newColors"/>.
        /// This will be applied to the <paramref name="toModify"/> image and will add the greyscale image of <paramref name="textureDetails"/> on top.
        /// This process is non destructive of <paramref name="toModify"/>
        /// WARNING: This method is BLOCKING and if it takes a while, will block your thread
        /// Use <see cref="ConvertTexture(Action{Texture2D}, Texture2D, Color[], Texture2D, int)"/> when possible to avoid blocking your thread!
        /// </summary>
        /// <param name="toModify"></param>
        /// <param name="newColors"></param>
        /// <param name="textureDetails"></param>
        /// <returns></returns>
        public static Texture2D ConvertTexture(Texture2D toModify, Color[] newColors, Texture2D textureDetails = null)
        {
#if !UNITY_EDITOR
            Debug.LogWarning("Texture2D ConvertTexture is a thread blocking function! Try not to use for your final project!");
#endif


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
                }
            }
            // Apply our changes!
            output.Apply();
            return output;
        }

        /// <summary>
        /// Converts a texture to a representation of 4 values provided in <paramref name="baseColors"/>.
        /// The imange being converted is <paramref name="toModify"/>.
        /// This process is non destructive of <paramref name="toModify"/>
        /// NOTE: This process is not perfect and will get muddy with mixes of colors!!
        /// WARNING: This method is BLOCKING and if it takes a while, will block your thread
        /// /// Use <see cref="ConvertTexture(Action{Texture2D}, Texture2D, Color[], Texture2D, int)"/> when possible to avoid blocking your thread!
        /// </summary>
        /// <param name="toModify"></param>
        /// <param name="baseColors"></param>
        /// <returns></returns>
        public static Texture2D GenerateBaseTexture(Texture2D toModify, Color[] baseColors)
        {
#if !UNITY_EDITOR
            Debug.LogWarning("Texture2D GenerateBaseTexture is a thread blocking function! Try not to use for your final project!");
#endif

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
                }
            }
            // Apply our changes!
            output.Apply();
            return output;
        }

        private static Vector4 forwardConvert(Vector4 patternColor, Vector4 detailColor, Color[] newColors)
        {
            if (newColors == null) 
            {
                Debug.LogError("No colors have been set for conversion");
                return Vector4.zero;
            }
            // We multiply each channel with the associated color
            Vector4 r = (newColors.Length >= 1 ? newColors[0] : Color.clear) * patternColor.x;
            Vector4 g = (newColors.Length >= 2 ? newColors[1] : Color.clear) * patternColor.y;
            Vector4 b = (newColors.Length >= 3 ? newColors[2] : Color.clear) * patternColor.z;

            // We add those colors together, then we apply the coat texture over that.
            Vector4 outColor = multiplyMatrix(patternColor, r, g, b) * (1 - (((detailColor.x + detailColor.y + detailColor.z) / 3) * detailColor.w));
            
            // We're just passing alpha for now
            outColor.w = patternColor.w;

            return outColor;
        }

        private static Vector3 multiplyMatrix(Vector3 v, Vector3 xS, Vector3 yS, Vector3 zS) 
        {
            Vector3 returnVector = new Vector3(
                xS.x * v.x + yS.x * v.y + zS.x * v.z,
                xS.y * v.x + yS.y * v.y + zS.y * v.z,
                xS.z * v.x + yS.z * v.y + zS.z * v.z
                );

            return returnVector;
        }

        private static float[] gaussianElimination(float[,] M) 
        {
            // Source:
            // https://www.codeproject.com/Tips/388179/Linear-Equation-Solver-Gaussian-Elimination-Csharp

            int rowCount = M.GetUpperBound(0) + 1;

            if (M.Length != rowCount * (rowCount + 1) || M == null) 
            {
                throw new ArgumentException("The algorithm must be provided with a (n x n+1) matrix.");
            }
            if (rowCount < 1) 
            {
                throw new ArgumentException("The matrix must at least have one row.");
            }

            float[] result = new float[rowCount];
            for (int i = 0; i < rowCount; i++)
                result[i] = 0;


            // Pivoting
            for (int col = 0; col + 1 < rowCount; col++) 
                if (M[col, col] == 0) // check for 0 coeffients
                {
                    // find non-0 coeffients
                    int swapRow = col + 1;
                    for (; swapRow < rowCount; swapRow++) if (M[swapRow, col] != 0) break;

                    if (M[swapRow, col] != 0) // non-0 found?
                    {
                        // Swap with above then
                        float[] tmp = new float[rowCount + 1];
                        for (int i = 0; i < rowCount + 1; i++)
                        {
                            tmp[i] = M[swapRow, i];
                            M[swapRow, i] = M[col, i];
                            M[col, i] = tmp[i];
                        }
                    }
                    else 
                        return result;
                           
                }

            // Elimination
            for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++)
            {
                for (int destRow = sourceRow + 1; destRow < rowCount; destRow++)
                {
                    float df = M[sourceRow, sourceRow];
                    float sf = M[destRow, sourceRow];
                    for (int i = 0; i < rowCount + 1; i++)
                        M[destRow, i] = M[destRow, i] * df - M[sourceRow, i] * sf;
                }
            }

            // back-insertion
            for (int row = rowCount - 1; row >= 0; row--)
            {
                float f = M[row, row];
                if (f == 0) return result;

                for (int i = 0; i < rowCount + 1; i++) M[row, i] /= f;
                for (int destRow = 0; destRow < row; destRow++)
                { M[destRow, rowCount] -= M[destRow, row] * M[row, rowCount]; M[destRow, row] = 0; }
            }

            for (int i = 0; i < rowCount; i++)
                result[i] = M[i, rowCount];

            return result;
        }

        private static Vector4 overOne(Vector4 input) 
        {
            return new Vector4(1/input.x, 1/input.y, 1/input.z, 1/input.w);
        }

        private static Vector4 backwardsConvert(Vector4 patternColor, Color[] baseColors) 
        {
            if (baseColors == null)
            {
                Debug.LogError("No colors have been set for conversion");
                return Vector4.zero;
            }

            Vector3 r = (Vector4)baseColors[0];
            Vector3 g = (Vector4)baseColors[1];
            Vector3 b = (Vector4)baseColors[2];

            Vector3 w = patternColor;

            float[,] matrix = new float[,] { { r.x, g.x, b.x, w.x }, { r.y, g.y, b.y, w.y }, { r.z, g.z, b.z, w.z } };

            float[] n = new float[] { 0, 0, 0 };

            try
            {
                n = gaussianElimination(matrix);
            }
            catch {}

            Vector4 colorOut = new Vector4(n[0], n[1], n[2], patternColor.w);
            
            return colorOut;
        }

        private static float[] VectorToFloat(Vector3 v) 
        {
            return new float[]{ v.x, v.y, v.z };
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