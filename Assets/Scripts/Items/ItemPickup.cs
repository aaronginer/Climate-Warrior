using InventorySystem;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class ItemPickup : MonoBehaviour
    {
        public GameObject inventoryDisplayObj;
        private InventoryDisplay _inventoryDisplay;
        public static event UnityAction<Vector3, ItemType> ItemPickedUp = delegate {  };
        
        private void Start()
        {
            _inventoryDisplay = inventoryDisplayObj.GetComponent<InventoryDisplay>();
        }

        public void OnTriggerStay2D(Collider2D col)
        {
            GameObject obj = col.gameObject;
            if (obj.CompareTag("Item"))
            {
                int type = int.Parse(obj.name);
                _inventoryDisplay.AddItem(int.Parse(obj.name));
                ItemPickedUp(obj.transform.position, (ItemType) type);
                Destroy(obj);
            }
        }
    }   
}
