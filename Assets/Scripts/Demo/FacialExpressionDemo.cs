using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpressionDemo : MonoBehaviour
{
    [SerializeField] GameObject eyes;
    [SerializeField] GameObject mouth;
    [SerializeField] Texture2D[] eyesExpressionTextures;
    [SerializeField] Texture2D[] mouthExpressionTextures;
    private MeshRenderer eyesMesh;
    private MeshRenderer mouthMesh;
    private Material eyesMaterial;
    private Material mouthMaterial;
    private void Start()
    {
        GetFaceMeshes();
        GetFaceMaterials();
    }

    private void GetFaceMeshes()
    {
        eyesMesh = eyes.GetComponent<MeshRenderer>();
        mouthMesh = mouth.GetComponent<MeshRenderer>();
    }

    private void GetFaceMaterials()
    {
        eyesMaterial = eyesMesh.materials[eyesMesh.materials.GetUpperBound(0)];
        mouthMaterial = mouthMesh.materials[mouthMesh.materials.GetUpperBound(0)];
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
