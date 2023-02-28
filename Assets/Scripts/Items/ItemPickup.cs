using InventorySystem;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class ItemPickup : MonoBehaviour
    {
        public GameObject inventoryDisplayObj;
        private InventoryDisplay _inventoryDisplay;
        public static event UnityAction<Vector3, ItemType> MissionItemPickUp = delegate {  };

        public static void ClearEventList()
        {
            MissionItemPickUp = null;
        }
        
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
                MissionItemPickUp(obj.transform.position, (ItemType) type);
                Destroy(obj);
            }
        }
    }   
}
