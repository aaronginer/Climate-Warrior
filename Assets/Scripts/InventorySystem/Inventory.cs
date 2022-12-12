using System;
using Items;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class Inventory
    {
        public readonly int Size;
        public InventorySlot[] slots;

        public Inventory(int size)
        {
            Size = size;
            slots = new InventorySlot[size];
        }

        public InventorySlot GetSlot(int index)
        {
            return slots[index];
        }

        public void SetSlot(int slotId, InventorySlot slot)
        {
            slots[slotId] = slot;
        }
        
        public void AddItem(ItemType itemType, int amount=1)
        {
            // if same item already existed in some slot, add amount to it
            foreach (InventorySlot slot in slots)
            {
                if (slot != null && slot.itemType == itemType)
                {
                    slot.amount += amount;
                    return;
                }
            }

            Debug.Log(this.GetHashCode());
            // add item in first available slot
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                {
                    slots[i] = new InventorySlot(amount, itemType);
                    return;
                }
            }
            Debug.Log("Inventory full.");
        }
    }
}