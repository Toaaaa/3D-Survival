using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;



public class DataManager : MonoBehaviour
{ // Newton 사용 버전
    public static DataManager Instance { get; private set; }
    private string dataPath; // 저장 경로

    private ItemDatas itemDatas;

    private void Awake()
    {
        if (Instance == null)
        { Instance = this; }
        else { Destroy(gameObject); }

        dataPath = Path.Combine(Application.dataPath + "/Json", "ItemData.Json");
        //Debug.Log($"datapath는 {dataPath}");
       
        LoadItemDataFromJson();
    }

    // 아이템 데이터 저장하기
    public void SaveItemDataToJson()
    {
        var json = JsonConvert.SerializeObject(itemDatas, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        File.WriteAllText(dataPath, json);
    }

    // 아이템 데이터 불러오기
    public void LoadItemDataFromJson()
    {
        if (File.Exists(dataPath))
        {
            var json = File.ReadAllText(dataPath);
            itemDatas = JsonConvert.DeserializeObject<ItemDatas>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });


            foreach (var itemData in itemDatas.itemsDatas)
            {
                if (itemData.drobPrefabName != null) { itemData.drobPrefab = UnityEngine.Resources.Load<GameObject>(itemData.drobPrefabName); }
                if (itemData.iconName != null) { itemData.icon = UnityEngine.Resources.Load<Sprite>(itemData.iconName); }
                if (itemData.equipPrefabName != null) { itemData.equipPrefab = UnityEngine.Resources.Load<GameObject>(itemData.equipPrefabName); }
                if (itemData.ConsumableType != null)
                {
                    
                    itemData.consumableTypes = new List<ConsumableType>();
                    foreach (var typeString in itemData.ConsumableType)
                    {
                        if (Enum.TryParse(typeString, out ConsumableType typeEnum))
                        {
                            
                            itemData.consumableTypes.Add(typeEnum);
                        }
                        else
                        {
                            Debug.LogWarning($"Unknown ConsumableType: {typeString}");
                        }
                    }
                }
            }
            //Debug.Log($"Loaded JSON: {json}");
        }
    }

    public ItemData GetItemDataByID(string id)
    {
        if (itemDatas == null || itemDatas.itemsDatas == null)
        {
            return null;
        }

        foreach (var item in itemDatas.itemsDatas)
        {
            if (item.itemKey == id)
            {
                return item;
            }
        }
        return null;
    }
}