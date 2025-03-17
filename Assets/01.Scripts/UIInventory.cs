using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public UIItemSlots[] uiSlots;
    public ItemSlot slot;

    public PlayerInventory playerInventory;
    public PlayerEquipment playerEquipment;


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

    public Button slotButton;

    private InputController controller;
    private PlayerCondition conditions;


    private void Start()
    {
        slotButton.onClick.AddListener(() => OnClick(slot.index));



        playerInventory = CharacterManager.Instance.Player.inventory;
        controller = CharacterManager.Instance.Player.input;
        conditions = CharacterManager.Instance.Player.condition;
        playerEquipment = CharacterManager.Instance.Player.equipment;

        //InputController에서 액션 생성
        controller.inventory += Toggle;

        playerInventory.InventoryUpdated += UpdateUI;

        // slots 밑에 딸린 itemslot.cs 달린 슬롯 개수 만큼 객체 생성
        uiSlots = new UIItemSlots[playerInventory.slotNumber];

        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i] = Instantiate(slotPrefab, slotPanel).GetComponent<UIItemSlots>();
            //slots[i] = slotPanel.GetChild(i).GetComponent<UIItemSlots>();
            uiSlots[i].Index = i;
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
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (playerInventory.slots[i].ItemData == null) return;

            if (uiSlots[i].Index == playerInventory.slots[i].index)
            {
                uiSlots[i].ItemData = playerInventory.slots[i].ItemData;
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

        bool isOpen = inventoryWindow.activeSelf;
        inventoryWindow.SetActive(!isOpen);
    }

    //public bool IsOpen()
    //{
    //    return inventoryWindow.activeInHierarchy;
    //}

    public void OnClick(int index)
    {
        // 슬롯 선택 로직 실행
        SelectItem(index);
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
        //for (int i = 0; i < selectedItem.ItemsConsumables.Length; i++)
        //{
        //    selectedItemStatName.text += selectedItem.ItemsConsumables[i].type.ToString() + "\n";
        //    selectedItemStatValue.text += selectedItem.ItemsConsumables[i].value.ToString() + "\n";
        //}

        if (selectedItem is ConsumableItemData consumableItem)
        {
            selectedItemStatName.text = consumableItem.consumableType.ToString();
            selectedItemStatValue.text = consumableItem.value.ToString();
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.type == ItemType.Equipable && uiSlots[index].equipped);
        
        dropButton.SetActive(true);
    }

    // 선택한 아이템을 쓰고난 뒤, 호출하는 함수
    void RemoveSelectedItem()
    {
        playerInventory.slots[selectedItemIndex].quantity--; // 슬롯 안의 아이템 개수가 하나 줄어든다.

        // 아이템 개수가 0개였다면
        if(playerInventory.slots[selectedItemIndex].quantity <= 0)
        {
            // 장비라면, 벗어준다.
            if (uiSlots[selectedItemIndex].equipped)
            {
                playerEquipment.UnEquip(playerInventory.slots[selectedItemIndex].ItemData);
            }

            // 장비가 아니라면, 슬롯을 비워준다.
            playerInventory.slots[selectedItemIndex].quantity = 0;
            playerInventory.slots[selectedItemIndex].ItemData = null;
            UpdateUI();
         }
        else
        {
            UpdateUI();
        }
    }

    // 1.사용하기버튼 

    public void OnUseButton()
    {
        if (selectedItem is ConsumableItemData consumableItem)
        {

            for (int i = 0; i < consumableItem.consumableType.Length; i++)
            {
                ConsumableType type = consumableItem.consumableType[i];
                float amount = consumableItem.value[i];

                if (conditions.Conditions.TryGetValue((ConditionType)type, out Condition condition))
                {
                    condition.ChangCondition(amount);
                }
            }
            //case consumabletype.health:
            //    if (conditions.conditions.trygetvalue(conditiontype.health, out condition health))
            //    {
            //        health.changcondition(consumableitem.value);
            //    }
            //    break;
            //case consumabletype.stamina:
            //    if (conditions.conditions.trygetvalue(conditiontype.stamina, out condition stamina))
            //    {
            //        stamina.changcondition(consumableitem.value);
            //    }
            //    break;
            //case consumabletype.water:
            //    if (conditions.conditions.trygetvalue(conditiontype.water, out condition water))
            //    {
            //        water.changcondition(consumableitem.value);
            //    }
            //    break;
            //case consumabletype.hunger:
            //    if (conditions.conditions.trygetvalue(conditiontype.hunger, out condition hunger))
            //    {
            //        hunger.changcondition(consumableitem.value);
            //    }
            //    break;
        
        }
        // 사용한 뒤에 selectItem을 초기화한다.
        RemoveSelectedItem();
        }
    

    // 2. 착용하기 버튼
    public void OnEquipButton()
    {
        selectedItem = playerInventory.slots[selectedItemIndex].ItemData;
        if (uiSlots[selectedItemIndex].equipped) { playerEquipment.UnEquip(selectedItem); }
        uiSlots[selectedItemIndex].equipped = true;
        playerEquipment.Equip(selectedItem);
        UpdateUI();
    }
    // 3. 해제하기 버튼
    public void OnUnEquipButton()
    {
        selectedItem = playerInventory.slots[selectedItemIndex].ItemData;
        uiSlots[selectedItemIndex].equipped = false;
        playerEquipment.UnEquip(selectedItem);
        UpdateUI();

        //if (selectedItemIndex == )
        //{
        //    SelectItem(selectedItemIndex);
        //}
    }
    // 버리기 버튼
    public void OnDropButton() 
    {
        playerInventory.ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

}
