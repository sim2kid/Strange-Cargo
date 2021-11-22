using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetics;
using TMPro;

public class CreatureDesignerController : MonoBehaviour
{
    GeneticRepository genePool;
    public TextMeshProUGUI partType;
    public TextMeshProUGUI bodyPart;
    public TextMeshProUGUI pattern;

    // Start is called before the first frame update
    void Start()
    {
        genePool = Utility.Toolbox.Instance.GenePool;
    }

    public void Build() 
    {
        
    }

    public void Next(string bodypart) 
    {
    
    }

    public void Last(string bodypart)
    {

    }
}
