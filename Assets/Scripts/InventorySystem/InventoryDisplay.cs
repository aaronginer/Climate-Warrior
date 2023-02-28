using System;
using Items;
using Missions;
using TMPro;
using UnityEngine;
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
        private readonly InventorySlot _handSlot = new(0, ItemType.None);
        private bool _active = true;

        private void OnDestroy()
        {
            GameStateManager.Instance.dialogueDisplay = null;
        }

        public void Start()
        {
            _inventory = GameStateManager.Instance.gameState.playerData.inventory;
            GameStateManager.Instance.inventoryDisplay = this;
            if (GameStateManager.Instance.BaseMission.State.stateID != (int) BaseMission.States.MissionActive)
            {
                _inventory.CleanInventory();
            }

            _itemImage = new Image[9];
            _itemText = new TextMeshProUGUI[9];

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
            if (!_active && !_handSlot.IsEmpty())
            {
                _inventory.AddItem(_handSlot.itemType, _handSlot.amount);
                _handSlot.Clear();
                UpdateInventory();
            }
            
            titleText.SetActive(_active);
            inventoryLayout.SetActive(_active);
            hand.SetActive(!_handSlot.IsEmpty());

            UIStateManager.UISM.uIState = _active ? UIState.Inventory : UIState.None;
        }

        // Updates the inventory UI elements
        // call this function every time something about the inventory changes
        private void UpdateInventory()
        {
            // --------------------------------------------------
            // sprite and text setters
            for (var i = 0; i < _itemImage.Length; i++)
            {
                var slot = _inventory.GetSlot(i);
                _itemImage[i].sprite = slot.GetSprite();
                _itemText[i].text = slot.GetAmountString();
            }

            _handImage.sprite = _handSlot.GetSprite();
            _handText.text = _handSlot.GetAmountString();
            hand.SetActive(_active);
            
            // --------------------------------------------------
            // enabled / disabled setters
            // display inventory settings
            for (var i = 0; i < _itemImage.Length; i++)
            {
                var slot = _inventory.GetSlot(i);
                _itemImage[i].color = slot.IsEmpty() ? new Color(1, 1,1, 0) : new Color(1, 1, 1, 1);
            }
                
            _handImage.color = _handSlot.IsEmpty() ? new Color(1, 1,1, 0) : new Color(1, 1, 1, 1);
            // --------------------------------------------------
        }

        public void SlotClick(int slotId)
        {
            var slot = _inventory.GetSlot(slotId);
            Debug.Log("Slot: " + slot.IsEmpty());
            Debug.Log("Hand: " + _handSlot.IsEmpty());
            
            if (_handSlot.IsEmpty() ^ slot.IsEmpty())
            {
                Debug.Log("xored");
                _handSlot.Exchange(slot);
                // update is handled by Update() function of HandScript, but that can be too slow so the item seems to jump
                _handScript.UpdatePosition();
            }
            
            UpdateInventory();
        }

        // wrapper that adds item to inventory and updates the display
        public void AddItem(int itemType)
        {
            _inventory.AddItem((ItemType) itemType);
            UpdateInventory();
        }
        
        // wrapper that clears the inventory and updates the display
        public void CleanInventory()
        {
            _inventory.CleanInventory();
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