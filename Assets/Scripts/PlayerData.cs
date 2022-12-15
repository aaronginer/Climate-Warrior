using System;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;


public enum MiniGame
{
    buildAWindTurbine,
    jumpAndRunCollectTurbineParts
}

[Serializable]
public class PlayerData
{
    public Inventory inventory;
    public String name;

    private Dictionary<MiniGame, int> missions = new Dictionary<MiniGame, int>(){
        { MiniGame.buildAWindTurbine, 0 },
        { MiniGame.jumpAndRunCollectTurbineParts, 0 }
    };


    public PlayerData(string name)
    {
        this.name = name;
        inventory = new Inventory(9);
    }

    public void PrintGameState()
    {
        Debug.Log("PlayerData:");
        Debug.Log(" - name: " + name);
    }
    public void completeMission(MiniGame miniGame) {
        missions[miniGame] = 1;
    }
    public bool checkMissionCompleted(MiniGame miniGame) {
        return (missions[miniGame] == 1);
    }
}