using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

[DisallowMultipleComponent]
public class Creature : MonoBehaviour, IProgress
{
    [SerializeField]
    public DNA dna;
    private IProgress textureController;

    public bool Finished => textureController.Finished;

    public float Report()
    {
        if (Finished)
            return 1;
        return textureController.Report();
    }

    void Start()
    {
        textureController = GetComponent<TextureConverter.TextureController>();
    }
}
