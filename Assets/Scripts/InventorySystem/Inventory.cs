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

        // ugly hack, change later (inventory is default initialized when saving it to disk)
        public void CleanInventory()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].amount == 0)
                {
                    slots[i] = null;
                }
            }
        }
        
        public void PrintInventory()
        {
            Debug.Log(GetHashCode());
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                {
                    Debug.Log(slots[i] + ": " + slots[i].itemType + " - " + slots[i].amount);
                }
            }
        }
    }
}