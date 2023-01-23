using UnityEngine;

public enum UIState
{
    None,
    Inventory,
    Dialogue
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

    public bool CanStartDialogue()
    {
        return uIState is UIState.None;
    }
}
