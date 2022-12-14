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
        if (col.gameObject.name == "StartTurbineMinigame")
            {
                if (!GameStateManager.GSM.gameState.playerData.checkMissionCompleted(0)) {
                    DialogueReader reader = new DialogueReader("blockeddialogue.txt");
                    dialogueObject.GetComponent<DialogueDisplay>().SetDialogueReader(reader);
                    dialogueObject.GetComponent<DialogueDisplay>().DialogueUpdate();
                }
                else {
                SceneManager.LoadScene("BuildAWindTurbine", LoadSceneMode.Single);
                }
            }

        if (Input.GetKeyDown("f"))
        {
            if (col.gameObject.name == "StartPathSign")
            {
                DialogueReader reader = new DialogueReader("pathdialogue.txt");
                dialogueObject.GetComponent<DialogueDisplay>().SetDialogueReader(reader);
                dialogueObject.GetComponent<DialogueDisplay>().DialogueUpdate();
            }
            else if (col.gameObject.name == "StartTurbineSign")
            {
                DialogueReader reader = new DialogueReader("turbinedialogue.txt");
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
