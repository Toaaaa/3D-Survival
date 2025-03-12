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
    // 추후 추가
}
[System.Serializable]

public class ItemDataConsumable // 소비 아이템 설정
{
    public ConsumableType type;
    public float value;
}

// 아이템 데이터 추가
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]


// 아이템 정보
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject drobPrefab;

    [Header("Stacking")] // 중복
    public bool canStack;
    public int maxStackAmount; 

    [Header("Consumable")]
    public ItemDataConsumable[] ItemsConsumables;
}
