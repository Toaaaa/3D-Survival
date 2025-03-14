using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProccesser : MonoBehaviour
{
    public ProcessItemSlot[] slots;
    public PlayerInventory inventory;
    public GameObject slotPrefab;

    public Image ProccessingItemIcon;
    public Image ProccessedItemIcon;
    public TextMeshProUGUI neededQuantityTxt;
    public Button proccessBtn;
    public Transform slotArea;

    public ItemData selectedItem;
    public int selectedItemIndex;

    private void Awake()
    {
        proccessBtn.onClick.AddListener(OnProccessBtn);
    }

    private void Start()
    {
        inventory = CharacterManager.Instance.Player.inventory;
        slots = new ProcessItemSlot[inventory.slots.Length];
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = Instantiate(slotPrefab, slotArea).GetComponent<ProcessItemSlot>();
            slots[i].Index = i;
        }
        UpdateProccesserUI();
        inventory.InventoryUpdated += UpdateProccesserUI;
    }

    public void OnProccessBtn()
    {
        if(selectedItem != null && selectedItem.proccessedItem != null && inventory.slots[selectedItemIndex].quantity>selectedItem.neededQuantity)
        {
            inventory.slots[selectedItemIndex].quantity -= selectedItem.neededQuantity;
            inventory.AddItem(selectedItem.proccessedItem);
        }
    }

    public void Select(int index)
    {
        selectedItemIndex = index;
        if (inventory.slots[index].ItemData != null)
        {
            ProccessingItemIcon.sprite = inventory.slots[index].ItemData.icon;
            selectedItem = inventory.slots[index].ItemData;
        }

        if (inventory.slots[index].ItemData.proccessedItem != null)
        {
            ProccessedItemIcon.sprite = inventory.slots[index].ItemData.proccessedItem.icon;
            neededQuantityTxt.text = $"X{inventory.slots[index].ItemData.neededQuantity}";
        }
    }

    private void OnDisable()
    {
        selectedItem = null;
        selectedItemIndex = -1;
        ProccessingItemIcon.sprite = null;
        ProccessedItemIcon.sprite = null;
        neededQuantityTxt.text = string.Empty;
    }


    public void UpdateProccesserUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            ItemSlot itemSlot = inventory.slots[i];
            if (itemSlot.ItemData != null)
            {
                slots[i].icon.sprite = itemSlot.ItemData.icon;
                slots[i].quantityText.text = itemSlot.quantity.ToString();
            }
            else
            {
                slots[i].icon.sprite = null;
                slots[i].quantityText.text = string.Empty;
            }
        }
    }
}
