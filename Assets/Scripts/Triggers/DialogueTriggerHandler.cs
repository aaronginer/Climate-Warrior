using System.Linq;
using Dialogue;
using TMPro;
using UnityEngine;

namespace Triggers
{
    
    public class DialogueTriggerHandler : MonoBehaviour
    {
        
        public GameObject dialogueObject;
        public GameObject textPopup;

        private TextMeshProUGUI _textPopupText;
        private DialogueTrigger _triggerScript;
        
        private DialogueDisplay _dialogueDisplay;

        private void Start()
        {
            _dialogueDisplay = dialogueObject.GetComponent<DialogueDisplay>();
            textPopup.SetActive(false);
            _textPopupText = textPopup.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Update()
        {
            if (!Input.GetKeyDown("f") || _triggerScript == null) return;
            
            textPopup.SetActive(false);
            TriggerActivate();
            _triggerScript = null;
        }
        
        public void OnTriggerEnter2D(Collider2D col)
        {
            _triggerScript = col.GetComponent<DialogueTrigger>();

            if (_triggerScript == null) return;
            
            if (_triggerScript.manual)
            {
                _textPopupText.text = _triggerScript.text;
                textPopup.SetActive(true);
            }
            else
            {
                TriggerActivate();    
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            _triggerScript = null;
            textPopup.SetActive(false);
        }

        private void TriggerActivate()
        {
            // return if the required mission for this trigger has not yet been 
            if (_triggerScript.requireCompleted != MiniGame.None
                && !GameStateManager.Instance.gameState.playerData.CheckMissionCompleted(_triggerScript.requireCompleted))
                return;

            _dialogueDisplay.StartNewDialogue(_triggerScript.dialoguePath);

            if (_triggerScript.destroy)
            {
                Destroy(_triggerScript.gameObject);
            }
        }
    }
}