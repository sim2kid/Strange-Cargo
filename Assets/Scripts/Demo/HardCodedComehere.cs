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
    FoodBowl location;

    public void ComeHere() 
    {
        if (location == null)
            return;
        Utility.Toolbox.Instance.CreatureList.First().AddTask(new Eat(location));
    }
}
