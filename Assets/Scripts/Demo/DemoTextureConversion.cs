using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[DisallowMultipleComponent]
public class DemoTextureConversion : MonoBehaviour
{
    [SerializeField]
    private Texture2D OverlayTexture = null;
    private Texture2D OriginalTexture;
    private const int MAX_COLORS = 3;
    [SerializeField]
    private bool forwardConvert = true;
    [SerializeField]
    private Color[] colors;

    private void OnValidate()
    {
        if (colors != null)
        {
            if (colors.Length > MAX_COLORS)
            {
                Debug.LogWarning($"You can only add upto {MAX_COLORS} colors for a conversion!");
                Array.Resize(ref colors, MAX_COLORS);
            }
        }
    }

    private void Start()
    {
        // Get the pattern texture from the material
        OriginalTexture = (Texture2D)this.GetComponent<Renderer>().material.mainTexture;

        // Call an Async method to convert wihtout clogging the main thread
        Invoke("Convert", 2);
    }

    public void Convert() 
    {
        Debug.Log($"Conversions Start!");
        // Take time to update the texture
        ModifyTexture(OriginalTexture, colors, OverlayTexture);
    }

    private void ReapplyTexture(Texture2D newTexture) 
    {
        // Set the main texture to our new texture
        this.GetComponent<Renderer>().material.mainTexture = newTexture;
        Debug.Log($"Conversion Finished!");
    }

    private void ModifyTexture(Texture2D toModify, Color[] newColors, Texture2D textureDetails = null) 
    {
        if (forwardConvert)
            StartCoroutine(Utility.TextureConversions.ConvertTexture(ReapplyTexture, toModify, newColors, textureDetails));
        else
            StartCoroutine(Utility.TextureConversions.GenerateBaseTexture(ReapplyTexture, toModify, newColors));
        
    }
}
