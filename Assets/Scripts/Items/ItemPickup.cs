using InventorySystem;
using UnityEngine;

namespace Items
{
    public class ItemPickup : MonoBehaviour
    {
        public GameObject inventoryDisplayObj;
        private InventoryDisplay _inventoryDisplay;
        
        private void Start()
        {
            _inventoryDisplay = inventoryDisplayObj.GetComponent<InventoryDisplay>();
        }

        public void OnTriggerStay2D(Collider2D col)
        {
            GameObject obj = col.gameObject;
            if (obj.CompareTag("Item"))
            {
                _inventoryDisplay.AddItem(int.Parse(obj.name));
                Destroy(obj);
            }
        }
    }   
}
