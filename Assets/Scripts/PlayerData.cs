using System;
using InventorySystem;
using UnityEngine;

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
}