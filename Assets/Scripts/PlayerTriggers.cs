using System;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTriggers : MonoBehaviour
{
    public GameObject dialogueObject;

    private GameObject _colliderObject = null;

    public void OnTriggerEnter2D(Collider2D col)
    {
        _colliderObject = col.gameObject;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        _colliderObject = null;
    }

    private void Update()
    {
        if (_colliderObject == null)
        {
            return;
        }
        
        switch (_colliderObject.gameObject.name)
        {
            case "StartJmpNRunMinigame":
                SceneManager.LoadScene(Constants.SceneNames.miniGameJumpAndRunCollectTurbine, LoadSceneMode.Single);
                break;
            case "StartTurbineMinigame" when !GameStateManager.GSM.gameState.playerData.checkMissionCompleted(MiniGame.jumpAndRunCollectTurbineParts):
            {
                DialogueReader reader = new DialogueReader("blockeddialogue.txt");
                dialogueObject.GetComponent<DialogueDisplay>().StartNewDialogue(reader);
                break;
            }
            case "StartTurbineMinigame":
                SceneManager.LoadScene(Constants.SceneNames.miniGameBuildAWindTurbine, LoadSceneMode.Single);
                break;
        }

        if (Input.GetKeyDown("f"))
        {
            switch (_colliderObject.gameObject.name)
            {
                case "StartPathSign":
                {
                    DialogueReader reader = new DialogueReader("pathdialogue.txt");
                    dialogueObject.GetComponent<DialogueDisplay>().StartNewDialogue(reader);
                    break;
                }
                case "StartTurbineSign":
                {
                    DialogueReader reader = new DialogueReader("turbinedialogue.txt");
                    dialogueObject.GetComponent<DialogueDisplay>().StartNewDialogue(reader);
                    break;
                }
                case "StartMayorDialogue":
                {
                    DialogueReader reader = new DialogueReader("mayordialogue.txt");
                    dialogueObject.GetComponent<DialogueDisplay>().StartNewDialogue(reader);
                    break;
                }
            }
        }
    }
}
