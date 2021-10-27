using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        //This script designates the attached Game Object as an interactable.

        public void Interact()
        {
            Debug.Log($"Interact method called for game object: {gameObject.name}");

            //Run some code depending on what we want this interactable to do
        }
    }
}