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
    GameObject sampleCreature;
    DNA oldDNA;
    DNA newDNA;

    // Start is called before the first frame update
    void Start()
    {
        genePool = Utility.Toolbox.Instance.GenePool;
        GenerateSampleCreature();
    }

    public void Build() 
    {
        if(newDNA != oldDNA)
        {
            if (sampleCreature != null)
            {
                GameObject.Destroy(sampleCreature);
                GenerateSampleCreature(newDNA);
            }
        }
    }

    private void GenerateSampleCreature()
    {
        sampleCreature = CreatureGeneration.CreateCreature();
        sampleCreature.transform.position = new Vector3(0, 0.5f, 0);
        sampleCreature.transform.Rotate(new Vector3(0, 180, 0));
        oldDNA = sampleCreature.GetComponent<Creature.CreatureController>().dna;
        newDNA = oldDNA;
    }

    private void GenerateSampleCreature(DNA _dna)
    {
        sampleCreature = CreatureGeneration.CreateCreature(_dna);
        sampleCreature.transform.position = new Vector3(0, 0.5f, 0);
        sampleCreature.transform.Rotate(new Vector3(0, 180, 0));
        oldDNA = sampleCreature.GetComponent<Creature.CreatureController>().dna;
        newDNA = oldDNA;
    }

    public void Next(string _bodypart) 
    {
        PartHash currentPart = new PartHash();
        foreach(PartHash partHash in oldDNA.BodyPartHashs)
        {
            if(partHash.Category == _bodypart)
            {
                currentPart = partHash;
            }
        }
        List<string> bodyPartNames = new List<string>();
        foreach(KeyValuePair<string, BodyPart> keyValuePair in genePool.GetPartList(_bodypart))
        {
            bodyPartNames.Add(keyValuePair.Key);
        }
        int currentPartIndex = bodyPartNames.IndexOf(currentPart.BodyPart);
        string newPartName = string.Empty;
        if (currentPartIndex < bodyPartNames.Count)
        {
            newPartName = bodyPartNames[currentPartIndex + 1];
        }
        else
        {
            newPartName = bodyPartNames[0];
        }
        newDNA.BodyPartHashs.Remove(currentPart);
        PartHash newPart = new PartHash
        {
            Category = _bodypart,
            BodyPart = newPartName,
            Pattern = currentPart.Pattern
        };
        newDNA.BodyPartHashs.Add(newPart);

    }

    public void Last(string _bodypart)
    {
        PartHash currentPart = new PartHash();
        foreach (PartHash partHash in oldDNA.BodyPartHashs)
        {
            if (partHash.Category == _bodypart)
            {
                currentPart = partHash;
            }
        }
        List<string> bodyPartNames = new List<string>();
        foreach (KeyValuePair<string, BodyPart> keyValuePair in genePool.GetPartList(_bodypart))
        {
            bodyPartNames.Add(keyValuePair.Key);
        }
        int currentPartIndex = bodyPartNames.IndexOf(currentPart.BodyPart);
        string newPartName = string.Empty;
        if (currentPartIndex > 0)
        {
            newPartName = bodyPartNames[currentPartIndex - 1];
        }
        else
        {
            newPartName = bodyPartNames[bodyPartNames.Count];
        }
        newDNA.BodyPartHashs.Remove(currentPart);
        PartHash newPart = new PartHash
        {
            Category = _bodypart,
            BodyPart = newPartName,
            Pattern = currentPart.Pattern
        };
        newDNA.BodyPartHashs.Add(newPart);
    }
}
