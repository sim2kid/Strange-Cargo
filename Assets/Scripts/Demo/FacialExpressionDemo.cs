using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpressionDemo : MonoBehaviour
{
    [SerializeField] Material happyFaceMaterial;
    [SerializeField] Material sadFaceMaterial;
    private MeshRenderer meshRenderer;
    private Material face;

    private void Start()
    {
        GetFace();
        MakeHappy();
    }

    public void GetFace()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        face = meshRenderer.materials[meshRenderer.materials.GetUpperBound(0)];
    }

    public void MakeHappy()
    {
        face.mainTexture = happyFaceMaterial.mainTexture;
    }

    public void MakeSad()
    {
        face.mainTexture = sadFaceMaterial.mainTexture;
    }
}
