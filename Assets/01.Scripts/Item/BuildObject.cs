using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildObject : MonoBehaviour
{
    public string name;
    public List<ItemData> needItem;
    public List<int> values;

    public Dictionary<ItemData, int> items;
    
    
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
