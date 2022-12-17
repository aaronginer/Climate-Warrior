using Items;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

namespace InventorySystem
{
    public class InventoryDisplay : MonoBehaviour
    {
        public GameObject titleText;
        public GameObject inventoryLayout;
        public GameObject[] itemObj;
        public GameObject hand;
        private HandScript _handScript;
        private Image[] _itemImage;
        private TextMeshProUGUI[] _itemText;
        private Image _handImage;
        private TextMeshProUGUI _handText;
        
        // playerscript as member

        private Inventory _inventory;
        private InventorySlot _handSlot;
        private bool _active = true;

        public void Start()
        {
            Debug.Log("Instantiating Display");
            
            _inventory = GameStateManager.GSM.gameState.playerData.inventory;
            _inventory.CleanInventory();

            _itemImage = new Image[9];
            _itemText = new TextMeshProUGUI[9];
            
            SpawnItem(new Vector3(0, 0, 0), ItemType.Bow);
            SpawnItem(new Vector3(0, 1, 0), ItemType.Sword);
            SpawnItem(new Vector3(0.5f, 1, 0), ItemType.Arrow);
            
            for (int i = 0; i < itemObj.Length; i++)
            {
                _itemImage[i] = itemObj[i].GetComponent<Image>();
                _itemText[i] = itemObj[i].GetComponentInChildren<TextMeshProUGUI>();
            }

            _handImage = hand.GetComponent<Image>();
            _handText = hand.GetComponentInChildren<TextMeshProUGUI>();
            _handScript = hand.GetComponent<HandScript>();

            UpdateInventory();
            ToggleInventory();
        }

        public void Update()
        {
            if (Input.GetKeyDown("e") && UIStateManager.UISM.CanToggleInventory())
            {
                ToggleInventory();
            }
        }
    
        private void ToggleInventory()
        {
            _active = !_active;
            // if there is an item left in hand when the inventory is closed, just add it to the inventory
            if (!_active && _handSlot != null)
            {
                _inventory.AddItem(_handSlot.itemType, _handSlot.amount);
                _handSlot = null;
                UpdateInventory();
            }
            
            titleText.SetActive(_active);
            inventoryLayout.SetActive(_active);
            hand.SetActive(_handSlot != null);

            UIStateManager.UISM.uIState = _active ? UIState.Inventory : UIState.None;
        }

        // Updates the inventory UI elements
        // call this function every time something about the inventory changes
        public void UpdateInventory()
        {
            // --------------------------------------------------
            // sprite and text setters
            for (var i = 0; i < _itemImage.Length; i++)
            {
                var slot = _inventory.GetSlot(i);
                if (slot != null)
                {
                    _itemImage[i].sprite = Item.GetSprite(slot.itemType);
                    _itemText[i].text = "" + slot.amount;
                }
            }
            if (_handSlot != null)
            {
                _handImage.sprite = Item.GetSprite(_handSlot.itemType);
                _handText.text = "" + _handSlot.amount;
                hand.SetActive(_active);
            }
            // --------------------------------------------------
            // enabled / disabled setters
            // display inventory settings
            for (var i = 0; i < _itemImage.Length; i++)
            {
                var slot = _inventory.GetSlot(i);
                var color = _itemImage[i].color;
                color.a = slot != null ? 1 : 0;
                _itemImage[i].color = color;
                _itemText[i].enabled = slot != null;
            }
                
            _handImage.enabled = _handSlot != null;
            _handText.enabled = _handSlot != null;
            // --------------------------------------------------
        }

        public void SlotClick(int slotId)
        {
            var slot = _inventory.GetSlot(slotId);
            if (_handSlot != null)
            {
                if (slot == null)
                {
                    _inventory.SetSlot(slotId, _handSlot);
                    _handSlot = null;
                }
            }
            else
            {
                if (slot != null)
                {
                    _handSlot = _inventory.GetSlot(slotId);
                    _inventory.SetSlot(slotId, null);
                    
                    // update is handled by Update() function of HandScript, but that can be too slow so the item seems to jump
                    _handScript.UpdatePosition();
                }
            }
            UpdateInventory();
        }

        // wrapper that adds item to inventory and updates the display
        public void AddItem(int itemType)
        {
            _inventory.AddItem((ItemType) itemType);
            UpdateInventory();
        }
        
        // move this to a more suitable place in the future!!!
        public static void SpawnItem(Vector3 position, ItemType itemType)
        {
            GameObject item = Instantiate(Resources.Load("Item")) as GameObject;
            if (item != null)
            {
                item.tag = "Item";
                item.name = "" + (int)itemType;
                item.transform.position = position;
                item.GetComponent<SpriteRenderer>().sprite = Item.GetSprite(itemType);
            }
        }
    }
}