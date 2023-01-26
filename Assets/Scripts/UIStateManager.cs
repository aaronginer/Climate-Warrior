using UnityEngine;

public enum UIState
{
    None,
    Inventory,
    Dialogue,
    MissionFinish,
    QuestMenu
}
public class UIStateManager : MonoBehaviour
{
    public static UIStateManager UISM { get; private set; }
    
    public UIState uIState = UIState.None;

    private void Awake()
    {
        if (UISM != null && UISM != this)
        {
            Destroy(this);
            return;
        }
        UISM = this;
        
        DontDestroyOnLoad(this);
    }

    public bool CanPlayerMove()
    {
        return uIState is UIState.None;
    }

    public bool CanToggleInventory()
    {
        return uIState is UIState.None or UIState.Inventory;
    }

    public bool CanToggleQuestMenu()
    {
        return uIState is UIState.None or UIState.QuestMenu;
    }

    public bool IsNone()
    {
        return uIState is UIState.None;
    }

    public bool CanOpenMenuOverlay()
    {
        return uIState is not UIState.MissionFinish;
    }
}
