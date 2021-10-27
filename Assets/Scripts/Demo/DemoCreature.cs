using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCreature : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject creature = Genetics.CreatureGeneration.CreateCreature();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
