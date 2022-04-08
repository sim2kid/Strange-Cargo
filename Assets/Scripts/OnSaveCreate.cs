using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Component;
using PersistentData.Saving;

public class OnSaveCreate : MonoBehaviour, ISaveable
{
    BoolListData data = new BoolListData()
    {
        _guid = "4d89a9f2-7287-47a9-99b6-3a70469040ac"
    };
    public ISaveData saveData { get => data; set => data = (BoolListData)value; }

    bool createdCreature = false;

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
    
    }

    private void CreateCreature() 
    {
        if(createdCreature) return;
        // Create Creature
        var creature = Genetics.CreatureGeneration.CreateCreature();
        creature.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(transform.position);
        //creature.transform.position = this.transform.position;
        createdCreature = true;
    }
}
