using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersistentData.Component;
using PersistentData.Saving;

public class OnSaveCreate : MonoBehaviour, ISaveable
{
    StringListData data = new StringListData()
    {
        _guid = "4d89a9f2-7287-47a9-99b6-3a70469040ac",
        StrList = new List<string>()
    };
    public ISaveData saveData { get => data; set => data = (StringListData)value; }

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

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void CreateCreature() 
    {
        // Create Creature
        var creature = Genetics.CreatureGeneration.CreateCreature();
        creature.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(transform.position);
        //creature.transform.position = this.transform.position;
    }
}
