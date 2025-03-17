using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource, // 자원
    Equipable, // 장비
    Consumable // 소비
}

public enum ConsumableType // 소비 아이템 효과
{
    Health,
    Stamina,
    Hunger,
    Water
    
}
// 아이템 데이터 추가 (newton x)
//[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

// 아이템 정보
[System.Serializable]
public class ItemData
{
    [Header("Info")]
    public string itemKey;
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public string iconName;
    public GameObject drobPrefab;
    public string drobPrefabName;

    [Header("Equip")]
    public GameObject equipPrefab;
    public string equipPrefabName;

    // 기존 위치
    //[Header("Stacking")] // 중복
    //public bool canStack;
    //public int maxStackAmount;

    //[Header("Consumable")]
    //public ConsumableItemData[] ItemsConsumables;

    //[Header("Equip")]
    //public GameObject equipPrefab;

     [Header("Resource")]
    public ItemData proccessedItem;
    public int neededQuantity;
}

public class ItemDatas
{
    public List<ItemData> itemsDatas;

}

[System.Serializable]

public class ConsumableItemData : ItemData
{
    //public ConsumableItemData[] ItemsConsumables;

    [Header("Stacking")] // 중복
    public bool canStack;
    public int maxStackAmount;

    public ConsumableType consumableType;
    public float value;
}
[System.Serializable]
public class ResourceItemData : ItemData
{
    [Header("Stacking")] // 중복
    public bool canStack;
    public int maxStackAmount;

}

[System.Serializable]
public class EquiptableItemData : ItemData
{
    //public int attackPower; equiptool 공격력/사거리 가져오기
}

// Newtonsoft.json 사용하지 않을 시에 필요함
//[System.Serializable]
//public class ItemDatas
//{
//    public ItemData[] itemDatas;
//}
