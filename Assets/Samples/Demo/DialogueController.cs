using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using TMPro;
using System.Linq;

public class DialogueController : MonoBehaviour
{
    DialogueManager manager;
    [SerializeField]
    GameObject dialogueParent;
    [SerializeField]
    TextMeshProUGUI dialogueText;
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    List<GameObject> optionButtons;
    [SerializeField]
    List<TextMeshProUGUI> optionText;
    [SerializeField]
    GameObject noOptionsNext;

    List<string> optionGuids;


    void Start()
    {
        // This is the manager we'll be talking to
        manager = GetComponent<DialogueManager>();
        // In theory, this manager will be swapped out a lot.
        dialogueParent.SetActive(false);

        StartConversation();
    }

    public void RenderCurrentNode() 
    {
        if (!manager.InConversation)
            EndConversation();
        dialogueText.text = manager.DialogueText;
        nameText.text = manager.Character;
        optionGuids = new List<string>();
        for (int i = 0; i < optionButtons.Count; i++)
            optionButtons[i].SetActive(false);
        noOptionsNext.SetActive(false);
        if (manager.DialogueOptions != null)
        {
            for (int i = 0; i < manager.DialogueOptions.Count; i++)
            {
                KeyValuePair<string, string> option = manager.DialogueOptions.ElementAt(i);
                optionButtons[i].SetActive(true);
                optionText[i].text = option.Key;
                optionGuids.Add(option.Value);
            }
        }
        if (optionGuids.Count == 0)
            noOptionsNext.SetActive(true);
    }

    public void StartConversation(DialogueManager newManager = null)
    {
        if (newManager != null)
            manager = newManager;
        if (manager != null)
        {
            dialogueParent.SetActive(true);
            RenderCurrentNode();
        }
    }

    public void EndConversation() 
    {
        dialogueParent.SetActive(false);
    }

    public void Next(int optionChoice = -1) 
    {
        if (optionChoice == -1)
        {
            manager.Next();
        }
        else if (optionGuids.Count > optionChoice)
        {
            manager.Next(optionGuids[optionChoice]);
        }
        else 
        {
            throw new System.Exception("Choice larger than listed choices returned.");
        }
        RenderCurrentNode();
    }

}
