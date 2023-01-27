using System;
using InventorySystem;
using UnityEngine;


public enum MiniGame
{
    None,
    buildAWindTurbine,
    jumpAndRunCollectTurbineParts,
    pipes,
}

[Serializable]
public class PlayerData
{
    public enum Gender
    {
        Male,
        Female,
        Diverse
    }
    
    public Inventory inventory;
    public Vector3 position;
    public string sceneName;
    public string name;
    public Gender gender;

    public int[] missions = new int[3];


    public PlayerData(string name)
    {
        this.name = name;
        sceneName = "Village";
        inventory = new Inventory(9);
    }

    public void PrintGameState()
    {
        Debug.Log("PlayerData:");
        Debug.Log(" - name: " + name);
    }
    public void CompleteMiniGame(MiniGame miniGame) {
        missions[(int)miniGame] = 1;
    }
    public bool CheckMiniGameCompleted(MiniGame miniGame) {
        return (missions[(int)miniGame] == 1);
    }
}