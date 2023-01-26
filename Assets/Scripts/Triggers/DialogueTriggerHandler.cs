using Dialogue;
using Mono.Cecil.Cil;
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
            if (!Input.GetKeyDown("f") || _triggerScript == null || !UIStateManager.UISM.IsNone()) return;
            
            textPopup.SetActive(false);
            TriggerActivate();
            _triggerScript = null;
        }
        
        public void OnTriggerEnter2D(Collider2D col)
        {
            _triggerScript = col.GetComponent<DialogueTrigger>();

            if (_triggerScript == null || !UIStateManager.UISM.IsNone()) return;
            
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
            if (_triggerScript.ShouldBlockTriggerByRequiredMission())
            {
                Debug.Log("BLOCKING DIALOGUE TRIGER BECAUSE OF MISSION CHECK");
                return;
            } 

            _dialogueDisplay.StartNewDialogue(_triggerScript.dialoguePath);

            if (_triggerScript.destroy)
            {
                Destroy(_triggerScript.gameObject);
            }
        }
    }
}