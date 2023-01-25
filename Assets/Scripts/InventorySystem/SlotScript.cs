using UnityEngine;

namespace InventorySystem
{
    public class SlotScript : MonoBehaviour
    {
        public int slotId;
        public GameObject inventoryDisplay;

        private InventoryDisplay _inventoryDisplay;
        
        private void Start()
        {
            _inventoryDisplay = inventoryDisplay.GetComponent<InventoryDisplay>();
        }

        public void OnSlotClick()
        {
            _inventoryDisplay.SlotClick(slotId);
        }
    }
}

