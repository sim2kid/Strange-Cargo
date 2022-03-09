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
    private string currentCategory;
    private string currentBodyPart;
    private string currentPattern;
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
        currentCategory = defaultPartHash.Category;
        category.text = defaultPartHash.Category;

        currentBodyPart = defaultPartHash.BodyPart;
        List<string> bodyParts = new List<string>();
        foreach (string key in genePool.GetPartList(currentCategory).Keys)
        {
            bodyParts.Add(key);
        }
        bodyPart.text = bodyParts.IndexOf(currentBodyPart).ToString();

        currentPattern = defaultPartHash.Pattern;
        List<string> patterns = new List<string>();
        foreach (KeyValuePair<string, BodyPart> keyValuePair in genePool.GetPartList(currentCategory))
        {
            if (keyValuePair.Key == currentBodyPart)
            {
                foreach (string pattern in keyValuePair.Value.Patterns)
                {
                    patterns.Add(genePool.GetPatternByName(pattern).Hash);
                }
            }
        }
        pattern.text = patterns.IndexOf(currentPattern).ToString();
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
        currentCategory = nextCategory;
        category.text = nextCategory;
        bodyPartHashToApply.Category = nextCategory;
        ResetBodyPart(nextCategory);
    }

    private void ResetBodyPart(string _category)
    {
        List<string> bodyParts = new List<string>();
        foreach (string key in genePool.GetPartList(_category).Keys)
        {
            bodyParts.Add(key);
        }
        bodyPart.text = "0";
        bodyPartHashToApply.BodyPart = bodyParts[0];
        ResetPattern(_category, bodyParts[0]);
    }

    private void ResetPattern(string _category, string _bodyPart)
    {
        List<string> patterns = new List<string>();
        foreach (KeyValuePair<string, BodyPart> keyValuePair in genePool.GetPartList(_category))
        {
            if (keyValuePair.Key == _bodyPart)
            {
                foreach (string pattern in keyValuePair.Value.Patterns)
                {
                    patterns.Add(genePool.GetPatternByName(pattern).Hash);
                }
            }
        }
        pattern.text = "0";
        bodyPartHashToApply.Pattern = patterns[0];
    }

    public void LastCategory()
    {
        List<string> categories = new List<string>();
        foreach(string key in genePool.Repository.Keys)
        {
            categories.Add(key);
        }
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
        currentCategory = lastCategory;
        category.text = lastCategory;
        bodyPartHashToApply.Category = lastCategory;
        ResetBodyPart(lastCategory);
    }

    public void NextBodyPart()
    {
        List<string> bodyParts = new List<string>();
        string currentCategory = category.text;
        foreach(string key in genePool.GetPartList(currentCategory).Keys)
        {
            bodyParts.Add(key);
        }
        int currentBodyPartIndex = bodyParts.IndexOf(currentBodyPart);
        string nextBodyPart = string.Empty;
        currentBodyPartIndex++;
        if (currentBodyPartIndex > bodyParts.Count - 1)
        {
            currentBodyPartIndex = 0;
        }
        nextBodyPart = bodyParts[currentBodyPartIndex];
        currentBodyPart = nextBodyPart;
        bodyPart.text = currentBodyPartIndex.ToString();
        bodyPartHashToApply.BodyPart = nextBodyPart;
        ResetPattern(currentCategory, nextBodyPart);
    }

    public void LastBodyPart()
    {
        List<string> bodyParts = new List<string>();
        string currentCategory = category.text;
        foreach(string key in genePool.GetPartList(currentCategory).Keys)
        {
            bodyParts.Add(key);
        }
        int currentBodyPartIndex = bodyParts.IndexOf(currentBodyPart);
        string lastBodyPart = string.Empty;
        currentBodyPartIndex--;
        if (currentBodyPartIndex < 0)
        {
            currentBodyPartIndex = bodyParts.Count - 1;
        }
        lastBodyPart = bodyParts[currentBodyPartIndex];
        currentBodyPart = lastBodyPart;
        bodyPart.text = currentBodyPartIndex.ToString();
        bodyPartHashToApply.BodyPart = lastBodyPart;
        ResetPattern(currentCategory, lastBodyPart);
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
                    patterns.Add(genePool.GetPatternByName(pattern).Hash);
                }
            }
        }
        int currentPatternIndex = patterns.IndexOf(currentPattern);
        string nextPattern = string.Empty;
        if (patterns.Count > 1)
        {
            currentPatternIndex++;

            if (currentPatternIndex > patterns.Count - 1)
            {
                currentPatternIndex = 0;
            }
        }
        else
        {
            currentPatternIndex = 0;
        }
        nextPattern = patterns[currentPatternIndex];
        currentPattern = nextPattern;
        pattern.text = currentPatternIndex.ToString();
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
                    patterns.Add(genePool.GetPatternByName(pattern).Hash);
                }
            }
        }
        int currentPatternIndex = patterns.IndexOf(currentPattern);
        string lastPattern = string.Empty;
        if (patterns.Count > 1)
        {
            currentPatternIndex--;
            if (currentPatternIndex < 0)
            {
                currentPatternIndex = 0;
            }
        }
        else
        {
            currentPatternIndex = 0;
        }
        lastPattern = patterns[currentPatternIndex];
        currentPattern = lastPattern;
        pattern.text = currentPatternIndex.ToString();
        bodyPartHashToApply.Pattern = lastPattern;
    }
}
