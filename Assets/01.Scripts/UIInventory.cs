using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlots[] slots;
    public GameObject inventoryWindow; // 인벤토리창
    public Transform slotPanel; // 슬롯패널

    public Transform dropPosition; // 버리기 위치

    [Header("Selected Item")]
    private ItemSlots selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemNDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private InputController controller;
    private PlayerCondition conditions;

    private void Start()
    {
        inventoryWindow.SetActive(false); // 창 초기화

        //controller = CharacterManager.Instance.Player.;
        //conditions = CharacterManager.Instance.Player.condition;
        //controller.inventory += Toggle;

        PlayerInventory.InventoryUpdated += UpdateUI;


        // slots 밑에 딸린 itemslot.cs 달린 슬롯 개수 만큼 객체 생성
        slots = new ItemSlots[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlots>();
            slots[i].Index = i;
            slots[i].uiInventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemNDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;
    }

    public void UpdateUI(ItemSlots[] items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemData != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    // 인벤토리 창 열고 닫기
    public void Toggle()
    {
        if (IsOpen())
        { inventoryWindow.SetActive(false); }
        else { inventoryWindow.SetActive(true); }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }
}
