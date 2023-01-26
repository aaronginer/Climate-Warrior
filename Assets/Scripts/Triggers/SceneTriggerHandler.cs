using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Triggers
{
    public class SceneTriggerHandler : MonoBehaviour
    {
        public GameObject textPopup;

        private TextMeshProUGUI _textPopupText;
        private SceneTrigger _triggerScript;

        private void Start()
        {
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
            _triggerScript = col.GetComponent<SceneTrigger>();

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
            // return if not in the correct mission state
            if (_triggerScript.ShouldBlockTriggerByRequiredMission())
            {
                Debug.Log("BLOCKING SCENE TRIGER BECAUSE OF MISSION CHECK");
                return;
            } 
            
            if (_triggerScript.setStartPosition)
            {
                GameStateManager.Instance.gameState.playerData.position = _triggerScript.startPosition;
            }
            
            SceneManager.LoadScene(_triggerScript.sceneName);
        }
    }
}

