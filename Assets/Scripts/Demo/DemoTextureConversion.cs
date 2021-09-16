using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTextureConversion : MonoBehaviour
{
    [SerializeField]
    private Texture2D OverlayTexture = null;
    private Texture2D TextureToModify;
    private const int MAX_COLORS = 3;
    [SerializeField]
    private Color[] colors;

    private void OnValidate()
    {
        if (colors.Length > MAX_COLORS) 
        {
            Debug.LogWarning($"You can only add upto {MAX_COLORS} colors for a conversion!");
            Array.Resize(ref colors, MAX_COLORS);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the pattern texture from the material
        TextureToModify = (Texture2D)this.GetComponent<Renderer>().material.mainTexture;

        // Set the main texture to our new texture
        this.GetComponent<Renderer>().material.mainTexture = ModifyTexture(TextureToModify, colors, OverlayTexture);
    }

    static Texture2D ModifyTexture(Texture2D toModify, Color[] newColors, Texture2D textureDetails = null) 
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

                // We multiply each channel with the associated color
                Vector4 r = (newColors.Length >= 1 ? newColors[0] : Color.clear) * patternColor.x;
                Vector4 g = (newColors.Length >= 2 ? newColors[1] : Color.clear) * patternColor.y;
                Vector4 b = (newColors.Length >= 3 ? newColors[2] : Color.clear) * patternColor.z;
                Vector4 a = (newColors.Length >= 4 ? newColors[3] : Color.clear) * patternColor.w;

                // We add those colors together, then we apply the coat texture over that.
                Vector4 outColor = (r + g + b + a) * (1 - (((detailColor.x + detailColor.y + detailColor.z) / 3) * detailColor.w));

                output.SetPixel(width, height, outColor);
            }
        }
        // Apply our changes!
        output.Apply();

        return output;
    }
    
}
