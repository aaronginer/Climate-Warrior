using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public enum ItemType
    {
        None,
        Turbine,
        Bow,
        Sword,
        Arrow,
        Cable,
        FloppyDisk,
        LockPick1,
        LockPick2,
        LockPick3,
        LockPick4,
        LockPick5,
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
            {ItemType.FloppyDisk, Resources.Load<Sprite>("Items/floppy_disk")},
            {ItemType.LockPick1, Resources.Load<Sprite>("Items/lockpick_1")},
            {ItemType.LockPick2, Resources.Load<Sprite>("Items/lockpick_2")},
            {ItemType.LockPick3, Resources.Load<Sprite>("Items/lockpick_3")},
            {ItemType.LockPick4, Resources.Load<Sprite>("Items/lockpick_4")},
            {ItemType.LockPick5, Resources.Load<Sprite>("Items/lockpick_5")},
        };

        public static Sprite GetSprite(ItemType itemType)
        {
            return ItemsDict.ContainsKey(itemType) ? ItemsDict[itemType] : null;
        }
    }   
}