using System;
using InventorySystem;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Inventory inventory;
    public String name;
    public int health;

    public PlayerData(string name, int health)
    {
        this.name = name;
        this.health = health;
        inventory = new Inventory(9);
        Debug.Log(inventory.GetHashCode());
    }

    public void PrintGameState()
    {
        Debug.Log("PlayerData:");
        Debug.Log(" - name: " + name);
        Debug.Log(" - health: " + health);
    }
}