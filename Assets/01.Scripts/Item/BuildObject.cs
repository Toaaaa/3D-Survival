using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildObject : MonoBehaviour
{
    public string name;
    public int originCapacy;
    private int capacy; 
    public GameObject previewPrefab;
    
    public CraftDictionary needItems;

    private void Awake()
    {
        capacy = originCapacy;
    }

    public string GetNameXValues()
    {
        string str = string.Empty;
        for (int i = 0; i < needItems.needCraft.Length; i++)
        {
            string need = $"{needItems.needCraft[i].itemData.displayName} X {needItems.needCraft[i].needValue}\n";
            str += need;
        }

        return str;
    }


    public void Demolition()
    {
        capacy -= 1;

        if (capacy < 0)
        {
            Destroy(gameObject);
        }
    }


}

[Serializable]
public class CraftDictionary
{
    [SerializeField]
    public NeedCraft[] needCraft;
}

[Serializable]
public class NeedCraft
{
    public ItemData itemData;
    public int needValue;

    public NeedCraft(ItemData itemData, int needValue)
    {
        this.itemData = itemData;
        this.needValue = needValue;
    }
}
