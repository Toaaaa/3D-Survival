using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;
    public PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = CharacterManager.Instance.Player.playerAttack;
    }

    public void Equip(ItemData equipItemData)
    {
        UnEquip(equipItemData);
        curEquip = Instantiate(equipItemData.equipPrefab, equipParent).GetComponent<Equip>();

        if (curEquip is EquipTool equipTool)
        {
            playerAttack.ChangePower(equipTool.attackPower);
            playerAttack.ChangeAttackDistance(equipTool.attackDistance);
        }
    }
    public void UnEquip(ItemData equipItemData)
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            // PlayerAttack의 attackPower 및 attackDistance 초기화 필요
            curEquip = null;
        }
    }
}
