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
    public Vector3 position;
    public String name;

    public int[] missions = new int[2];


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
    public void CompleteMission(MiniGame miniGame) {
        missions[(int)miniGame] = 1;
    }
    public bool CheckMissionCompleted(MiniGame miniGame) {
        return (missions[(int)miniGame] == 1);
    }
}