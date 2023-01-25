using System;
using Items;

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
    }
}