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
            case "StartTurbineMinigame" when !GameStateManager.GSM.gameState.playerData.CheckMissionCompleted(MiniGame.jumpAndRunCollectTurbineParts):
            {
                dialogueObject.GetComponent<DialogueDisplay>().StartNewDialogue("blockeddialogue.txt", true);
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
                    dialogueObject.GetComponent<DialogueDisplay>().StartNewDialogue("pathdialogue.txt");
                    break;
                }
                case "StartTurbineSign":
                {
                    dialogueObject.GetComponent<DialogueDisplay>().StartNewDialogue("turbinedialogue.txt");
                    break;
                }
                case "StartMayorDialogue":
                {
                    dialogueObject.GetComponent<DialogueDisplay>().StartNewDialogue("mayordialogue.txt");
                    break;
                }
            }
        }
    }
}
