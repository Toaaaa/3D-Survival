using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildObject : MonoBehaviour
{
    public string name;
    public List<ItemData> needItem;
    public List<int> values;
    
    public GameObject previewPrefab;

    // public List<NeedCraft> needItems;
    public CraftDictionary needItems;

    public string GetNameXValues()
    {
        string str = string.Empty;
        for (int i = 0; i < needItem.Count; i++)
        {
            string need = $"{needItem[i].displayName} X {values[i]}\n";
            
            str += need;
        }

        return str;
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
