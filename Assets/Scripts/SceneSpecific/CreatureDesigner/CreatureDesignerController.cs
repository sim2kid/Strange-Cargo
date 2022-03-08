using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genetics;
using TMPro;

public class CreatureDesignerController : MonoBehaviour
{
    GeneticRepository genePool;
    public TextMeshProUGUI category;
    public TextMeshProUGUI bodyPart;
    public TextMeshProUGUI pattern;
    public GameObject creatureSpawnLocation;
    GameObject sampleCreature;
    DNA oldDNA;
    PartHash bodyPartHashToApply;

    // Start is called before the first frame update
    void Start()
    {
        genePool = Utility.Toolbox.Instance.GenePool;
        Utility.Toolbox.Instance.Pause.SetPause(true);
        GenerateSampleCreature();
        PartHashSetup();
    }

    private void PartHashSetup()
    {
        PartHash defaultPartHash = oldDNA.BodyPartHashs[0];
        bodyPartHashToApply = defaultPartHash;
        category.text = defaultPartHash.Category;
        bodyPart.text = defaultPartHash.BodyPart;
        pattern.text = defaultPartHash.Pattern;
    }

    private void GenerateSampleCreature()
    {
        sampleCreature = CreatureGeneration.CreateCreature();
        SampleCreatureSetup();
    }

    private void GenerateSampleCreature(DNA _dna)
    {
        sampleCreature = CreatureGeneration.CreateCreature(_dna);
        SampleCreatureSetup();
    }

    private void SampleCreatureSetup()
    {
        oldDNA = sampleCreature.GetComponent<Creature.CreatureController>().dna;
        sampleCreature.transform.SetPositionAndRotation(creatureSpawnLocation.transform.position, creatureSpawnLocation.transform.rotation);
    }

    public void Apply()
    {
        if(!oldDNA.BodyPartHashs.Contains(bodyPartHashToApply))
        {
            DNA newDNA = oldDNA;
            PartHash bodyPartHashToRemove = new PartHash();
            foreach(PartHash bodyPartHash in oldDNA.BodyPartHashs)
            {
                if(bodyPartHash.Category == bodyPartHashToApply.Category)
                {
                    bodyPartHashToRemove = bodyPartHash;
                }
            }
            newDNA.BodyPartHashs.Remove(bodyPartHashToRemove);
            newDNA.BodyPartHashs.Add(bodyPartHashToApply);
            if (sampleCreature != null)
            {
                Destroy(sampleCreature);
                GenerateSampleCreature(newDNA);
            }
        }
    }

    public void NextCategory()
    {
        List<string> categories = new List<string>();
        foreach(string key in genePool.Repository.Keys)
        {
            categories.Add(key);
        }
        string currentCategory = category.text;
        int currentCategoryIndex = categories.IndexOf(currentCategory);
        string nextCategory = string.Empty;
        if(currentCategoryIndex >= categories.Count - 1)
        {
            nextCategory = categories[0];
        }
        else
        {
            nextCategory = categories[currentCategoryIndex + 1];
        }
        category.text = nextCategory;
        bodyPartHashToApply.Category = nextCategory;
    }

    public void LastCategory()
    {
        List<string> categories = new List<string>();
        foreach(string key in genePool.Repository.Keys)
        {
            categories.Add(key);
        }
        string currentCategory = category.text;
        int currentCategoryIndex = categories.IndexOf(currentCategory);
        string lastCategory = string.Empty;
        if(currentCategoryIndex <= 0)
        {
            lastCategory = categories[categories.Count - 1];
        }
        else
        {
            lastCategory = categories[currentCategoryIndex - 1];
        }
        category.text = lastCategory;
        bodyPartHashToApply.Category = lastCategory;
    }

    public void NextBodyPart()
    {
        List<string> bodyParts = new List<string>();
        string currentCategory = category.text;
        foreach(string key in genePool.GetPartList(currentCategory).Keys)
        {
            bodyParts.Add(key);
        }
        string currentBodyPart = bodyPart.text;
        int currentBodyPartIndex = bodyParts.IndexOf(currentBodyPart);
        string nextBodyPart = string.Empty;
        if(currentBodyPartIndex >= bodyParts.Count - 1)
        {
            nextBodyPart = bodyParts[0];
        }
        else
        {
            nextBodyPart = bodyParts[currentBodyPartIndex + 1];
        }
        bodyPart.text = nextBodyPart;
        bodyPartHashToApply.BodyPart = nextBodyPart;
    }

    public void LastBodyPart()
    {
        List<string> bodyParts = new List<string>();
        string currentCategory = category.text;
        foreach(string key in genePool.GetPartList(currentCategory).Keys)
        {
            bodyParts.Add(key);
        }
        string currentBodyPart = bodyPart.text;
        int currentBodyPartIndex = bodyParts.IndexOf(currentBodyPart);
        string lastBodyPart = string.Empty;
        if(currentBodyPartIndex <= 0)
        {
            lastBodyPart = bodyParts[bodyParts.Count - 1];
        }
        else
        {
            lastBodyPart = bodyParts[currentBodyPartIndex - 1];
        }
        bodyPart.text = lastBodyPart;
        bodyPartHashToApply.BodyPart = lastBodyPart;
    }

    public void NextPattern()
    {
        List<string> patterns = new List<string>();
        string currentCategory = category.text;
        string currentBodyPart = bodyPart.text;
        foreach(KeyValuePair<string,BodyPart> keyValuePair in genePool.GetPartList(currentCategory))
        {
            if(keyValuePair.Key == currentBodyPart)
            {
                foreach(string pattern in keyValuePair.Value.Patterns)
                {
                    patterns.Add(pattern);
                }
            }
        }
        string currentPattern = pattern.text;
        int currentPatternIndex = patterns.IndexOf(currentPattern);
        string nextPattern = string.Empty;
        if(currentPatternIndex >= patterns.Count - 1)
        {
            nextPattern = patterns[0];
        }
        else
        {
            nextPattern = patterns[currentPatternIndex + 1];
        }
        pattern.text = nextPattern;
        bodyPartHashToApply.Pattern = nextPattern;
    }

    public void LastPattern()
    {
        List<string> patterns = new List<string>();
        string currentCategory = category.text;
        string currentBodyPart = bodyPart.text;
        foreach (KeyValuePair<string, BodyPart> keyValuePair in genePool.GetPartList(currentCategory))
        {
            if (keyValuePair.Key == currentBodyPart)
            {
                foreach (string pattern in keyValuePair.Value.Patterns)
                {
                    patterns.Add(pattern);
                }
            }
        }
        string currentPattern = pattern.text;
        int currentPatternIndex = patterns.IndexOf(currentPattern);
        string lastPattern = string.Empty;
        if (currentPatternIndex <= 0)
        {
            lastPattern = patterns[patterns.Count - 1];
        }
        else
        {
            lastPattern = patterns[currentPatternIndex - 1];
        }
        pattern.text = lastPattern;
        bodyPartHashToApply.Pattern = lastPattern;
    }
}
