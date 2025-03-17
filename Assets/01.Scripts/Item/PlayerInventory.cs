using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

//using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    // Player 오브젝트에 붙이고,
    // CharacterManager.Instance.Player.playerInventory로 접근
    public Transform dropPosition;

    public ItemSlot[] slots; 
    public int slotNumber = 12; 

    public InputController controller;
    public PlayerCondition condition;

    public ItemData ItemData;

    public event Action InventoryUpdated;

    private void Awake()
    {
        //CharacterManager.Instance.Player.AddItem += AddItem;

        slots = new ItemSlot[slotNumber];

        InventoryUpdated += CheckEmptySlot;

        //dropPosition = CharacterManager.Instance.Player.dropPosition;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot(null, 0, i);
        }
    }

    void Update()
    {
        
    }

    public void AddItem(ItemData itemData)
    {
        // 여러개 가질 수 있다면
        if (itemData.type == ItemType.Consumable || itemData.type == ItemType.Resource)
        {
            // 아이템 데이터를 넣어서 같은 아이템데이터를 가진 슬롯을 슬롯에 넣어준다.
            ItemSlot slot = GetItemStack(itemData);

            // 슬롯이 있다면
            if (slot != null)
            {
                // 슬롯의 개수를 늘리고,
                slot.quantity++;
                // InventoryUpdated 액션을 invoke해준다.
                TriggerUpdateUI();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot(); // 아이템이 없는 슬롯을 가져온다.

        // 빈 슬롯이 있다면
        if (emptySlot != null)
        {
            // 그 슬롯의 아이이템은 이것이 된다.
            emptySlot.ItemData = itemData;
            emptySlot.quantity = 1;
            // // InventoryUpdated 액션을 invoke해준다.
            TriggerUpdateUI();
            return;
        }
        else
        {
            // 땅에 버린다.
            ThrowItem(itemData);
        } 
    }
    public void ThrowItem(ItemData itemdata)
    {
        Instantiate(itemdata.drobPrefab, dropPosition.position, Quaternion.identity);
        TriggerUpdateUI();
    }

    ItemSlot GetItemStack(ItemData data)
    {
        if (data is ConsumableItemData consumableItem)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                // 슬롯의 아이템과 넣으려는 아이템이 같고 / 아이템 개수가 최대를 넘지 않을때 
                if (slots[i].ItemData == data && slots[i].quantity < consumableItem.maxStackAmount)
                {
                    return slots[i];
                }
            }
        }
        return null;
    }

    // 빈 슬롯 찾아서 반환 메서드
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 아이템 안들어있을 때 그 슬롯 반환
            if (slots[i].ItemData == null)
            { return slots[i]; }
        }
        return null;
    }

    public void TriggerUpdateUI()
    {
        InventoryUpdated?.Invoke();
    }

    public void CheckEmptySlot()
    {
        for(int i =0; i < slots.Length; i++)
        {
            if (slots[i].ItemData != null)
            {
                if (slots[i].quantity == 0)
                {
                    slots[i].ItemData = null;
                }
            }
        }
    }
}

[System.Serializable]
public class ItemSlot
{
    public ItemData ItemData;
    UIInventory uiInventory;
    public int quantity;
    public int index;

    public ItemSlot(ItemData itemData, int quantity, int _index)
    {
        ItemData = itemData;
        this.quantity = quantity;
        index = _index;
    }

    public void OnClick(int index)
    {
        uiInventory.SelectItem(index);
    }
}