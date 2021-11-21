using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Creature;
using Creature.Stats;

public class ShowCreatureNeeds : MonoBehaviour
{
    public Slider[] sliders;
    public CreatureController creature;

    void Start()
    {
        try
        {
            creature = Utility.Toolbox.Instance.CreatureList[0];
        }
        catch 
        {
            Console.LogWarning($"There are no creatures in the current instance of the game and can not be displayed.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (creature == null)
        {
            creature = Utility.Toolbox.Instance.CreatureList[0];
            return;
        }
        for (int i = 0; i < sliders.Length && i < creature.needs.Count; i++) 
        {
            sliders[i].value = creature.needs[i];
        }
    }
}
