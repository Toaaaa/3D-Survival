using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public UIItemSlots[] uiSlots;
    public PlayerInventory playerInventory;


    public GameObject inventoryWindow; // 인벤토리창
    public Transform slotPanel; // 슬롯패널
    public GameObject slotPrefab;

    public Transform dropPosition; // 버리기 위치

    [Header("Selected Item")]
    public ItemData selectedItem;
    public int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemNDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    public Action onClickUseBtn;


    private InputController controller;
    private PlayerCondition conditions;

  

    private void Start()
    {
        inventoryWindow.SetActive(false); // 창 초기화

        playerInventory = CharacterManager.Instance.Player.inventory;
        controller = CharacterManager.Instance.Player.input;
        conditions = CharacterManager.Instance.Player.condition;


        //InputController에서 액션 생성
        //controller.inventory += Toggle;
        

        playerInventory.InventoryUpdated += UpdateUI;


        // slots 밑에 딸린 itemslot.cs 달린 슬롯 개수 만큼 객체 생성
        uiSlots = new UIItemSlots[playerInventory.slotNumber];

        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i] = Instantiate(slotPrefab, slotPanel).GetComponent<UIItemSlots>();
            //slots[i] = slotPanel.GetChild(i).GetComponent<UIItemSlots>();
            //slots[i].Index = i;
            //slots[i].uiInventory = this;
            //slots[i].Clear();
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

    public void UpdateUI()
    {
        //for (int i = 0; i < slots.Length; i++)
        //{
        //    if (slots[i].ItemData != null)
        //    {
        //        //slots[i].ItemData = [i].ItemData; 
        //        //slots[i].quantity = itemSlots[i].quantity;
        //        slots[i].Set(i); 
        //    }
        //    else
        //    {
        //        slots[i].Clear();
        //    }
        //}

        for(int i=0; i<uiSlots.Length; i++)
        {
            if (playerInventory.slots[i].ItemData != null)
            {
                uiSlots[i].icon.sprite = playerInventory.slots[i].ItemData.icon;
                uiSlots[i].quantityText.text = playerInventory.slots[i].quantity.ToString();
            }
            else
            {
                uiSlots[i].icon.sprite = null; 
                uiSlots[i].quantityText.text = string.Empty;
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

    public void SelectItem(int index)
    {
        if (uiSlots[index].ItemData == null) return;

        selectedItem = uiSlots[index].ItemData;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemNDescription.text = selectedItem.description;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        // 소비 아이템일 때만 StatName, StatValue 나타내기
        for (int i = 0; i < selectedItem.ItemsConsumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.ItemsConsumables[i].type.ToString();
            selectedItemStatValue.text += selectedItem.ItemsConsumables[i].value.ToString();
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.type == ItemType.Equipable && uiSlots[index].equipped);
        
        dropButton.SetActive(true);
    }
    
    // 1.사용하기버튼 (구현예정)

    public void OnUseButton()
    {
        
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.ItemsConsumables.Length; i++)
            {
                switch (selectedItem.ItemsConsumables[i].type)
                {
                    // 소비 타입에 따른 함수 호출
                    //case ConsumableType.Health:
                    //    conditions.EatHealthItem(selectedItem.ItemsConsumables[i].value); break;
                    //case ConsumableType.Stamina:
                    //    conditions.EatStaminaItem(selectedItem.ItemsConsumables[i].value); break;
                    //case ConsumableType.Speed:
                    //    conditions.EatSpeedItem(); break;
                }
            } 
        }
        }

    // 2. 착용하기 버튼
    public void OnEquipButton()
    {

    }
    // 3. 해제하기 버튼
    public void OnUnEquipButton()
    {

    }
    // 버리기 버튼
    public void OnDropButton(int index) 
    {
        
    }

    
}
