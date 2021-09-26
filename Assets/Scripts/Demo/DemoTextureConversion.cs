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
    private bool forwardConvert = true;
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

    Texture2D ModifyTexture(Texture2D toModify, Color[] newColors, Texture2D textureDetails = null) 
    {
        if (forwardConvert)
            return Utility.TextureConversions.ConvertTexture(toModify, newColors, textureDetails);
        else
            return Utility.TextureConversions.GenerateBaseTexture(toModify, newColors);
    }
    
}
