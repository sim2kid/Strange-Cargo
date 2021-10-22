using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpressionDemo : MonoBehaviour
{
    //[SerializeField] Material happyFaceMaterial;
    //[SerializeField] Material sadFaceMaterial;
    [SerializeField] Material[] facialExpressions;
    private MeshRenderer meshRenderer;
    private Material face;

    private void Start()
    {
        GetFace();
        SetFacialExpressionToDefault();
    }

    public void GetFace()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        face = meshRenderer.materials[meshRenderer.materials.GetUpperBound(0)];
    }

    public void SetFacialExpressionToDefault()
    {
        face.mainTexture = facialExpressions[0].mainTexture;
    }

    public void SetFacialExpressionTo(int facialExpressionIndex)
    {
        face.mainTexture = facialExpressions[facialExpressionIndex].mainTexture;
    }

    /*public void MakeHappy()
    {
        face.mainTexture = happyFaceMaterial.mainTexture;
    }

    public void MakeSad()
    {
        face.mainTexture = sadFaceMaterial.mainTexture;
    }*/
}
