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
            Debug.LogWarning("You can only add upto 3 colors for a conversion!");
            Array.Resize(ref colors, MAX_COLORS);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TextureToModify = (Texture2D)this.GetComponent<Material>().mainTexture;
        // Set the '_MainTex' to our new texture
        this.GetComponent<Material>().SetTexture("_MainTex", ModifyTexture(TextureToModify, colors, OverlayTexture));
    }

    static Texture ModifyTexture(Texture2D toModify, Color[] newColors, Texture2D textureDetails = null) 
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

        for (int width = 0; width < output.width; width++) 
        {
            for (int height = 0; height < output.height; height++) 
            {
                //TODO: Actual conversions go here
            }
        }

        return (Texture)output;
    }

    
}
