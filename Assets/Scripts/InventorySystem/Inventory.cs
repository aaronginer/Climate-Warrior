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
            for (var i = 0; i < size; i++)
            {
                slots[i] = new InventorySlot(0, ItemType.None);
            }
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
                if (!slot.IsEmpty() && slot.itemType == itemType)
                {
                    slot.amount += amount;
                    return;
                }
            }

            // add item in first available slot
            foreach (var t in slots)
            {
                if (t.IsEmpty())
                {
                    t.SetItem(itemType, amount);
                    return;
                }
            }
            
            Debug.Log("Inventory full.");
        }

        // ugly hack, change later (inventory is default initialized when saving it to disk)
        public void CleanInventory()
        {
            foreach (var t in slots)
            {
                t.Clear();
            }
        }

        public int CountInventoryItem(ItemType itemType)
        {
            foreach (var t in slots)
            {
                if (t.itemType == itemType)
                {
                    return t.amount;
                }
            }

            return 0;
        }
        
        public void PrintInventory()
        {
            Debug.Log(GetHashCode());
            for (int i = 0; i < slots.Length; i++)
            {
                if (!slots[i].IsEmpty())
                {
                    Debug.Log(slots[i] + ": " + slots[i].itemType + " - " + slots[i].amount);
                }
            }
        }
    }
}