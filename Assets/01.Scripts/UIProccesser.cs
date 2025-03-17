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
    private ItemData proccessedData;

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
        if (proccessedData != null && inventory.slots[selectedItemIndex].quantity >= selectedItem.neededQuantity)
        {
            inventory.slots[selectedItemIndex].quantity -= selectedItem.neededQuantity;
            inventory.AddItem(proccessedData);
        }
    }

    public void Select(int index)
    {
        if (inventory.slots[index].ItemData == null) return;

        selectedItemIndex = index;
        if (inventory.slots[index].ItemData != null)
        {
            ProccessingItemIcon.sprite = inventory.slots[index].ItemData.icon;
            selectedItem = inventory.slots[index].ItemData;
        }
        else
        {
            UIReset();
            return;
        }

        if (inventory.slots[index].ItemData.type == ItemType.Resource && inventory.slots[index].ItemData.nextItemIdx >0)
        {
            proccessedData = DataManager.Instance.GetItemDataByID(inventory.slots[index].ItemData.nextItemIdx.ToString());
            ProccessedItemIcon.sprite = proccessedData.icon;
            neededQuantityTxt.text = $"X{inventory.slots[index].ItemData.neededQuantity}";
        }

        if(proccessedData == null)
        {
            ProccessedItemIcon.sprite = null;
            neededQuantityTxt.text = string.Empty;
        }
    }

    private void UIReset()
    {
        selectedItem = null;
        selectedItemIndex = -1;
        proccessedData = null;
        ProccessingItemIcon.sprite = null;
        ProccessedItemIcon.sprite = null;
        neededQuantityTxt.text = string.Empty;
    }
    private void OnDisable()
    {
        UIReset();
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
