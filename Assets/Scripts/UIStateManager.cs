using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Newtonsoft.Json.JsonSerializer;

public enum UIState
{
    None,
    Inventory,
    Dialogue,
    WalkableDialogue
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
        return uIState is UIState.None or UIState.WalkableDialogue;
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
