using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetics;
using TMPro;

public class CreatureDesignerController : MonoBehaviour
{
    GeneticRepository genePool;
    public TextMeshProUGUI accessoriesIndex;
    public TextMeshProUGUI bodiesIndex;
    public TextMeshProUGUI earsIndex;
    public TextMeshProUGUI hatsIndex;
    public TextMeshProUGUI headsIndex;
    public TextMeshProUGUI hornsIndex;
    public TextMeshProUGUI legsIndex;
    public TextMeshProUGUI masksIndex;
    public TextMeshProUGUI tailsIndex;
    private int[] indexs;

    // Start is called before the first frame update
    void Start()
    {
        indexs = new int[6];
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
