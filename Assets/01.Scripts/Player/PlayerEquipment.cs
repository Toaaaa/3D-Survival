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
        UnEquip();
        curEquip = Instantiate(equipItemData.equipPrefab, equipParent).GetComponent<Equip>();

        if (curEquip is EquipTool equipTool)
        {
            CharacterManager.Instance.Player.handler.isWeopon = true;
            playerAttack.ChangePower(equipTool.attackPower);
            playerAttack.ChangeAttackDistance(equipTool.attackDistance);
        }
    }
    public void UnEquip()
    {
        if (curEquip != null)
        {               
            Destroy(curEquip.gameObject);
            
            CharacterManager.Instance.Player.handler.isWeopon = false;
            curEquip = null;
        }
    }

}
