using TMPro;
using UnityEngine;

namespace UI
{
    public class QuestDisplay : MonoBehaviour
    {
        public GameObject questMenuLayout;
        public TextMeshProUGUI questDescriptionText;
        public TextMeshProUGUI questNameText;
        
        // playerscript as member
        private bool _active = true;

        public void Start()
        {
            _active = false;
            questMenuLayout.SetActive(_active);
            

        }

        public void Update()
        {
            if (Input.GetKeyDown("q") && UIStateManager.UISM.CanToggleQuestMenu())
            {
                if (GameStateManager.Instance.CurrentMission != null)
                {
                    questNameText.text = GameStateManager.Instance.CurrentMission.Name;
                    questDescriptionText.text = GameStateManager.Instance.CurrentMission._description;
                }
                else
                {
                    questNameText.text = "";
                    questDescriptionText.text = "You have no active mission. Explore the village or talk to the mayor when you are ready for your next mission!";
                }
                
                ToggleQuestMenu();
            }
        }

        private void ToggleQuestMenu()
        {
            _active = !_active;
            
            questMenuLayout.SetActive(_active);

            UIStateManager.UISM.uIState = _active ? UIState.QuestMenu : UIState.None;
        } 
    }
}