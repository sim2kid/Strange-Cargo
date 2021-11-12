using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Creature;
using Creature.Task;

public class HardCodedComehere : MonoBehaviour
{
    [SerializeField]
    Transform location;

    public void ComeHere() 
    {
        if (location == null)
            return;
        Utility.Toolbox.Instance.CreatureList.First().AddTask(new ComeHere(location));
    }
}
