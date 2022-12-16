using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

public class TestScript : MonoBehaviour
{
    public GameObject dialogue;
    public string dialogueFile;

    private DialogueDisplay _dialogueDisplay;
    
    void Start()
    {
        DialogueReader r = new DialogueReader(dialogueFile);
        _dialogueDisplay = dialogue.GetComponent<DialogueDisplay>();
        _dialogueDisplay.GetComponent<DialogueDisplay>().StartNewDialogue(r);
    }
}
