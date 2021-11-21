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
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (creature == null)
        {
            try
            {
                creature = Utility.Toolbox.Instance.CreatureList[0];
            }
            catch
            {

            }
            return;
        }
        for (int i = 0; i < sliders.Length && i < creature.needs.Count; i++) 
        {
            sliders[i].value = creature.needs[i];
        }
    }
}
