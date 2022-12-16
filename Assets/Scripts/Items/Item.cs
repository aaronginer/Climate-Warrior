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
        Turbine
    }

    sealed class Item
    {
    
        static readonly Dictionary<ItemType, Sprite> ItemsDict = new()
        {
            {ItemType.Turbine, Resources.Load<Sprite>("Items/wind_turbine")},
        };

        public static Sprite GetSprite(ItemType itemType)
        {
            return ItemsDict.ContainsKey(itemType) ? ItemsDict[itemType] : null;
        }
    }   
}