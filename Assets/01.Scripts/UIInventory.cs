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
            uiSlots[i].quantityText.text = string.Empty;
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

    // 참고:슬롯 정렬 메서드

    // 아이템이 들어왔을 때
    //public void Set(int index)
    //{
    //    icon.gameObject.SetActive(true);
    //    icon.sprite = uiInventory.playerInventory.slots[index].ItemData.icon;
    //    quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // 아이템 수량 없으면 표시 안 함

    //    // 방어코드
    //    if(outline != null ) {outline.enabled = equipped;}
    //}

    //// 아이템이 빠질 때
    //public void Clear()
    //{
    //    //uiInventory.playerInventory.slots[index].ItemData = null;
    //    icon.gameObject.SetActive(false);
    //    quantityText.text = string.Empty;
    //}

    public void UpdateUI()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
           //if (playerInventory.slots[i].ItemData == null) return;

            if (playerInventory.slots[i].ItemData != null && uiSlots[i].Index == playerInventory.slots[i].index)
            {
                uiSlots[i].ItemData = playerInventory.slots[i].ItemData;
                uiSlots[i].icon.sprite = playerInventory.slots[i].ItemData.icon;
                uiSlots[i].quantityText.text = playerInventory.slots[i].quantity > 1 ? playerInventory.slots[i].quantity.ToString() : string.Empty;
            }
            else
            {
                uiSlots[i].ItemData = null;
                uiSlots[i].icon.sprite = null;
                uiSlots[i].quantityText.text = string.Empty;
            }
        }
        ClearSelectedItemWindow();
    }

    // 인벤토리 창 열고 닫기
    public void Toggle()
    {
        bool isOpen = inventoryWindow.activeSelf;
        inventoryWindow.SetActive(!isOpen);
    }


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
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.ConsumableType.Count; i++)
            {
                selectedItemStatName.text += selectedItem.consumableTypes[i].ToString() + "\n";
                selectedItemStatValue.text += selectedItem.value[i].ToString() + "\n";
            }
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
                playerEquipment.UnEquip();
            }

            // 장비가 아니라면, 슬롯을 비워준다.
            playerInventory.slots[selectedItemIndex].quantity = 0;
            playerInventory.slots[selectedItemIndex].ItemData = null;
            ClearSelectedItemWindow();
        }
            UpdateUI();
    }

    // 1.사용하기버튼 

    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {

            for (int i = 0; i < selectedItem.ConsumableType.Count; i++)
            {
                switch(selectedItem.consumableTypes[i])
                {
                    case ConsumableType.Health:
                        if (conditions.Conditions.TryGetValue(ConditionType.Health, out Condition health))
                        {
                            health.ChangCondition(selectedItem.value[i]);
                        }
                        break;
                    case ConsumableType.Stamina:
                        if (conditions.Conditions.TryGetValue(ConditionType.Stamina, out Condition stamina))
                        {
                            stamina.ChangCondition(selectedItem.value[i]);
                        }
                        break;
                    case ConsumableType.Water:
                        if (conditions.Conditions.TryGetValue(ConditionType.Water, out Condition water))
                        {
                            water.ChangCondition(selectedItem.value[i]);
                        }
                        break;
                    case ConsumableType.Hunger:
                        if (conditions.Conditions.TryGetValue(ConditionType.Hunger, out Condition hunger))
                        {
                            hunger.ChangCondition(selectedItem.value[i]);
                        }
                        break;
                }   
            }
            // 사용한 뒤에 selectItem을 초기화한다.
            RemoveSelectedItem();
        }
    }


    // 2. 착용하기 버튼
    public void OnEquipButton()
    {
        selectedItem = playerInventory.slots[selectedItemIndex].ItemData;
        if (uiSlots[selectedItemIndex].equipped) { playerEquipment.UnEquip(); }
            else
            {
                // 모든 슬롯의 equipped 상태를 false로 초기화
                // 중복 착용 방지 위함
                for (int i = 0; i < uiSlots.Length; i++)
                {
                    uiSlots[i].equipped = false;
                }
                uiSlots[selectedItemIndex].equipped = true;
            playerEquipment.Equip(selectedItem);
        }

        UpdateUI();

        SelectItem(selectedItemIndex);

    }
    // 3. 해제하기 버튼
    public void OnUnEquipButton()
    {
        selectedItem = playerInventory.slots[selectedItemIndex].ItemData;
        playerEquipment.UnEquip();
        UpdateUI();
        uiSlots[selectedItemIndex].equipped = false;
    }
    // 4. 버리기 버튼
    public void OnDropButton() 
    {
        if (selectedItem == null) { return; }
        if (selectedItem.drobPrefab != null) { playerInventory.ThrowItem(selectedItem); }
        RemoveSelectedItem();
    }
}
