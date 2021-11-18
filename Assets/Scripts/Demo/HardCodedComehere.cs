using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Creature;
using Creature.Task;
using Creature.Stats;

public class HardCodedComehere : MonoBehaviour
{
    [SerializeField]
    Transform location;
    [SerializeField]
    FoodBowl bowl;

    public void ComeHere() 
    {
        if (location == null)
            return;
        Utility.Toolbox.Instance.CreatureList.First().AddTask(new GoHere(location, 20));
    }
    public void Eat()
    {
        if (location == null)
            return;
        Utility.Toolbox.Instance.CreatureList.First().AddTask(new Eat(bowl));
    }
}
