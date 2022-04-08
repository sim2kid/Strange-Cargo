using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Brain
{
    public class Memory
    {
        public Option option;
        public float timeEnded;

        public Memory(Option option) 
        {
            this.option = option;
        }
    }
}
