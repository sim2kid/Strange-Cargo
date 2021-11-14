using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Stats
{
    public class WaterBowl : FoodBowl
    {
        public override Creature.Task.ITask RelatedTask => new Creature.Task.Drink(this);
    }
}