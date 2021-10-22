using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpressionDemo : MonoBehaviour
{
    [SerializeField] GameObject eyes;
    [SerializeField] GameObject mouth;
    [SerializeField] Texture2D[] eyesExpressionTextures;
    [SerializeField] Texture2D[] mouthExpressionTextures;
    private Material eyesMaterial;
    private Material mouthMaterial;
    private void Start()
    {
        GetFaceMaterials();
    }

    private void GetFaceMaterials()
    {
        eyesMaterial = eyes.GetComponent<MeshRenderer>().material;
        mouthMaterial = mouth.GetComponent<MeshRenderer>().material;
    }

    public void SetExpressionToDefault()
    {
        eyesMaterial.mainTexture = eyesExpressionTextures[0];
        mouthMaterial.mainTexture = mouthExpressionTextures[0];
    }

    public void SetEyesExpressionTo(int expressionIndex)
    {
        eyesMaterial.mainTexture = eyesExpressionTextures[expressionIndex];
    }

    public void SetMouthExpressionTo(int expressionIndex)
    {
        mouthMaterial.mainTexture = mouthExpressionTextures[expressionIndex];
    }
}
