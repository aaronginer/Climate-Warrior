using System;
using System.Collections.Generic;
using System.Reflection;
using InventorySystem;
using UnityEditor;
using UnityEngine;

namespace Items
{
    public enum ItemType
    {
        Sword,
        Boat,
        Bow
    }

    sealed class Item
    {
    
        static readonly Dictionary<ItemType, Sprite> ItemsDict = new()
        {
            {ItemType.Sword, Resources.Load<Sprite>("Items/diamond_sword")},
            {ItemType.Boat, Resources.Load<Sprite>("Items/acacia_boat")},
            {ItemType.Bow, Resources.Load<Sprite>("Items/bow")}
        };

        public static Sprite GetSprite(ItemType itemType)
        {
            return ItemsDict.ContainsKey(itemType) ? ItemsDict[itemType] : null;
        }
    }   
}