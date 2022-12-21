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
            "TestDialogue",
            "Panel1Sign"
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
            bool automatic = _automatic.Contains(col.name);
            bool manual = _manual.Contains(col.name);
            if (!(automatic || manual)) return;

            _colliderObject = col.gameObject;
            if (manual)
            {
                startDialogueText.GetComponentInChildren<TextMeshProUGUI>().text = col.name.Contains("Sign") ? signText : dialogueText;
                startDialogueText.SetActive(true);
            }
            else
            {
                HandleAutomaticDialogue();
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

        private void HandleManualDialogue()
        {
            if (!Input.GetKeyDown("f")) return;
            
            startDialogueText.SetActive(false);
            switch (_colliderObject.gameObject.name)
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
                case "TestDialogue":
                {
                    _dialogueDisplay.StartNewDialogue("Missions/Sabotage/sabotage_2");
                    break;
                }
                case "Panel1Sign":
                {
                    _dialogueDisplay.StartNewDialogue("Missions/Sabotage/panel_1");
                    Destroy(_colliderObject);
                    break;
                }
            }
        }
    }
}