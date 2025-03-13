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


    private ItemSlots[] slots;

    public InputController controller;
    public PlayerCondition condition;

    public ItemData ItemData;

    public static event Action<ItemSlots[]> InventoryUpdated;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddItem(ItemData itemData)
    {
        if (ItemData.canStack)
        {
            ItemSlots slot = GetItemStack(itemData);

            // 중복가능한 존재하는 슬롯 찾기
            if (slot != null)
            {
                slot.quantity++;
                TriggerUpdateUI();
                return;
            }
        }

        ItemSlots emptySlot = GetEmptySlot();

        // 빈 슬롯 찾기
        if (emptySlot != null)
        {
            emptySlot.ItemData = itemData;
            emptySlot.quantity = 1;
            TriggerUpdateUI();
            return;
        }
    }

    ItemSlots GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯의 아이템과 넣으려는 아이템이 같고 / 아이템 개수가 최대를 넘지 않을때 
            if (slots[i].ItemData == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 빈 슬롯 찾아서 반환 메서드
    ItemSlots GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 아이템 안들어있을 때 그 슬롯 반환
            if (slots[i].ItemData == null)
            { return slots[i]; }
        }
        return null;
    }

    private void TriggerUpdateUI()
    {
        InventoryUpdated?.Invoke(slots);
    }
}
