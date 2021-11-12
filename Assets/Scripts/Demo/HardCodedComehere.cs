using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Creature;

public class HardCodedComehere : MonoBehaviour
{
    [SerializeField]
    Transform location;

    public void ComeHere() 
    {
        if (location == null)
            return;
        Utility.Toolbox.Instance.CreatureList.First().gameObject.GetComponent<NavMeshMovement>().MoveTo(location);
    }
}
