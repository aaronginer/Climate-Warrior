using System;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTriggers : MonoBehaviour
{
    public GameObject dialogueObject;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "StartJmpNRunMinigame")
            {
                SceneManager.LoadScene("JumpAndRun", LoadSceneMode.Single);
            }

        if (Input.GetKeyDown("f"))
        {
            if (col.gameObject.name == "StartTurbineMinigame")
            {
                SceneManager.LoadScene("BuildAWindTurbine", LoadSceneMode.Single);
            }
            else if (col.gameObject.name == "StartPathSign")
            {
                DialogueReader reader = new DialogueReader("pathdialogue.txt");
                dialogueObject.GetComponent<DialogueDisplay>().SetDialogueReader(reader);
                dialogueObject.GetComponent<DialogueDisplay>().DialogueUpdate();
            }
            else if (col.gameObject.name == "StartMayorDialogue")
            {
                DialogueReader reader = new DialogueReader("mayordialogue.txt");
                dialogueObject.GetComponent<DialogueDisplay>().SetDialogueReader(reader);
                dialogueObject.GetComponent<DialogueDisplay>().DialogueUpdate();
            }
        }
    }
}
