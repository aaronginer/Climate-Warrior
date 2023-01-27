using TMPro;
using UnityEngine;

namespace QuestSystem
{
public class QuestDisplay : MonoBehaviour
{
    public GameObject questMenuLayout;
    public TextMeshProUGUI questText;
    
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
            if (GameStateManager.Instance.CurrentMission != null) {
                questText.text = GameStateManager.Instance.CurrentMission._description;
            }
            else {
                questText.text = "You have no active mission. Explore the village in search of a new quest!";
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