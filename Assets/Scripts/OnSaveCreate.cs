using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Component;
using PersistentData.Saving;
using Animatics;

public class OnSaveCreate : MonoBehaviour, ISaveable
{
    BoolListData data = new BoolListData()
    {
        _guid = "4d89a9f2-7287-47a9-99b6-3a70469040ac"
    };
    public ISaveData saveData { get => data; set => data = (BoolListData)value; }

    Dictionary<string, bool> Events => data.BoolData;

    private void Start()
    {
        Events.Add("createdCreature", false);
        Events.Add("playedStartCutscene", false);
    }

    public void PostDeserialization()
    {
        data.PostDeserialize();
        StartCoroutine(ProcessContent());
    }

    public void PreDeserialization() { }

    public void PreSerialization() 
    {
        data.PreSerialize();
    }

    private IEnumerator ProcessContent() 
    {
        yield return new WaitForFixedUpdate();
        CreateCreature();
    }

    public void TryOpeningCutScene() 
    {
        if (Events["playedStartCutscene"]) return;

        Console.Log("Running starting cutscene.");
        // Run start cutscene
        OpeningScene os = new OpeningScene();
        Instantiate(os);
        os.OnFinish.AddListener(() => {
            Events["playedStartCutscene"] = true;
        });
    }

    private void CreateCreature() 
    {
        if(Events["createdCreature"]) return;
        // Create Creature
        var creature = Genetics.CreatureGeneration.CreateCreature();
        creature.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(transform.position);
        //creature.transform.position = this.transform.position;
        Events["createdCreature"] = true;
    }
}
