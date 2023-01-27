using System;
using Items;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class InventorySlot
    {
        public int amount;
        public ItemType itemType;

        public InventorySlot(int amount, ItemType itemType)
        {
            this.amount = amount;
            this.itemType = itemType;
        }

        public bool IsEmpty()
        {
            return itemType == ItemType.None;
        }

        public void SetItem(ItemType iType, int a)
        {
            itemType = iType;
            amount = a;
        }

        public void Clear()
        {
            itemType = ItemType.None;
            amount = 0;
        }

        public void Exchange(InventorySlot other)
        {
            ItemType t = itemType;
            int a = amount;

            itemType = other.itemType;
            amount = other.amount;

            other.itemType = t;
            other.amount = a;
        }

        public Sprite GetSprite()
        {
            return IsEmpty() ? null : Item.GetSprite(itemType);
        }
        
        public string GetAmountString()
        {
            return IsEmpty() ? "" : "" + amount;
        }
    }
}