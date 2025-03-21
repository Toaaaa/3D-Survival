using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip

// 무기 정보
{
    public ItemData itemData;

    public float attackRate;
    public bool attacking;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    
    public float attackDistance; // 무기 장착 시 상승하는 플레이어 공격 사거리
    public float attackPower; // 무기 장착 시 상승하는 플레이어 공격력 = 몬스터에게 입히는 데미지
}
