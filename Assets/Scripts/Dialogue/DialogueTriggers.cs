using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    
    public class DialogueTriggers : MonoBehaviour
    {
        
        public GameObject dialogueObject;
        public GameObject startDialogueText;

        private GameObject _colliderObject = null;
        private DialogueDisplay _dialogueDisplay;
        
        private readonly string[] _automatic =
        {
            "StartTurbineMinigame"
        };

        private readonly string[] _manual =
        {
            "StartPathSign",
            "StartTurbineSign",
            "StartMayorDialogue",
        };

        private const string signText = "F - Read";
        private const string dialogueText = "F - Talk!";
        
        
        private void Start()
        {
            _dialogueDisplay = dialogueObject.GetComponent<DialogueDisplay>();
            startDialogueText.SetActive(false);
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            var automatic = _automatic.Contains(col.name);
            var manual = _manual.Contains(col.name);

            _colliderObject = col.gameObject;
            if (manual || (GameStateManager.Instance.CurrentMission?.IsManualDialogueTrigger(_colliderObject) ?? false))
            {
                startDialogueText.GetComponentInChildren<TextMeshProUGUI>().text = col.name.Contains("Sign") ? signText : dialogueText;
                startDialogueText.SetActive(true);
            }
            else if (automatic)
            {
                HandleAutomaticDialogue();
                
            }
            else if (GameStateManager.Instance.CurrentMission?.IsAutomaticDialogueTrigger(_colliderObject) ?? false)
            {
                GameStateManager.Instance.CurrentMission.HandleAutomaticDialogueTriggers(_colliderObject, _dialogueDisplay);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            _colliderObject = null;
            startDialogueText.SetActive(false);
        }

        public void Update()
        {
            if (_colliderObject != null)
            {
                HandleManualDialogue();
            }
        }

        // handle dialogue triggers that are not specific to missions
        private void HandleAutomaticDialogue()
        {
            switch (_colliderObject.name)
            {
                case "StartTurbineMinigame" when !GameStateManager.Instance.gameState.playerData.CheckMissionCompleted(MiniGame.jumpAndRunCollectTurbineParts):
                {
                    dialogueObject.GetComponent<DialogueDisplay>().StartNewDialogue("blockeddialogue");
                    break;
                }
            }
        }

        // handle dialogue triggers that are not specific to missions
        private void HandleManualDialogue()
        {
            if (!Input.GetKeyDown("f")) return;
            
            startDialogueText.SetActive(false);
            switch (_colliderObject.name)
            {
                case "StartPathSign":
                {
                    _dialogueDisplay.StartNewDialogue("pathdialogue");
                    break;
                }
                case "StartTurbineSign":
                {
                    _dialogueDisplay.StartNewDialogue("turbinedialogue");
                    break;
                }
                case "StartMayorDialogue":
                {
                    _dialogueDisplay.StartNewDialogue("mayordialogue");
                    break;
                }
                default:
                    GameStateManager.Instance.CurrentMission?.HandleManualDialogueTriggers(_colliderObject, _dialogueDisplay);
                    break;
            }
        }
    }
}