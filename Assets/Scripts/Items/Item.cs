using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public enum ItemType
    {
        Turbine,
        Bow,
        Sword,
        Arrow,
        Cable,
    }

    sealed class Item
    {
    
        static readonly Dictionary<ItemType, Sprite> ItemsDict = new()
        {
            {ItemType.Turbine, Resources.Load<Sprite>("Items/wind_turbine")},
            {ItemType.Bow, Resources.Load<Sprite>("Items/bow_standby")},
            {ItemType.Sword, Resources.Load<Sprite>("Items/iron_sword")},
            {ItemType.Arrow, Resources.Load<Sprite>("Items/arrow")},
            {ItemType.Cable, Resources.Load<Sprite>("Items/cable")},
        };

        public static Sprite GetSprite(ItemType itemType)
        {
            return ItemsDict.ContainsKey(itemType) ? ItemsDict[itemType] : null;
        }
    }   
}