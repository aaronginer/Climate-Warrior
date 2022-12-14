using System;
using Items;
using UnityEngine.Windows.WebCam;

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