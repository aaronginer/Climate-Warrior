using System;
using InventorySystem;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Inventory inventory;
    public String name;
    int[] missions = new int[] {0, 0};


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
    public void completeMission(int id) {
        missions[id] = 1;
    }
    public bool checkMissionCompleted(int id) {
        return (missions[id] == 1);
    }
}