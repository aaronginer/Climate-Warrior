using System;
using InventorySystem;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Inventory inventory;
    public String name;

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
}