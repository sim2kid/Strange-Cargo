using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Component;
using PersistentData.Saving;
using Animatics;

public class OnSaveCreate : MonoBehaviour, ISaveable
{
    [SerializeField]
    BoolListData data = new BoolListData()
    {
        _guid = "4d89a9f2-7287-47a9-99b6-3a70469040ac"
    };
    public ISaveData saveData { get => data; set => data = (BoolListData)value; }

    Dictionary<string, bool> Events => data.BoolData;

    private void Start()
    {
        EnsureKey("createdCreature", false);
        EnsureKey("playedStartCutscene", false);
        GameObject.FindObjectOfType<UI.ScreenLoading>()
            .End.AddListener(TryOpeningCutScene);
    }

    private void EnsureKey(string key, bool defaultValue) 
    {
        if (!Events.TryGetValue(key, out bool value))
            Events.Add(key, defaultValue);
    }
    public void PostDeserialization()
    {
        StartCoroutine(ProcessContent());
    }

    public void PreDeserialization() { }

    public void PreSerialization() { }

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
        GameObject os = new GameObject("Opening Scene");
        os.AddComponent<OpeningScene>().OnFinish.AddListener(() => {
            Events["playedStartCutscene"] = true;
            Destroy(os);
        });
    }

    private void CreateCreature() 
    {
        if(Events["createdCreature"]) return;

        Console.Log("Creating First Creature.");
        // Create Creature
        var creature = Genetics.CreatureGeneration.CreateCreature();
        creature.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(transform.position);
        //creature.transform.position = this.transform.position;
        Events["createdCreature"] = true;
    }
}
