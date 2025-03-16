using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;


// Newton 방식 X
//public class DataManager : MonoBehaviour
//{
//    public ItemData itemData;
//    public ItemDatas itemDatas;
//    // 다른 데이터 클래스 추가하면 여기에 참조

//    private string dataPath; // 데이터 경로

//    // 싱글톤
//    private static DataManager instance;
//    public static DataManager Instance
//    {
//        get 
//        { 
//            if (instance == null)
//            {
//                instance = new GameObject("DataManager").GetComponent<DataManager>();
//            }
//            return instance; 
//        }
//    }

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }

//        dataPath = Path.Combine(Application.persistentDataPath + "itemData.json");
//        Debug.Log(dataPath); // 경로 확인용

//        // 파일이 없을 때 임시 파일
//        if (!File.Exists(dataPath)) { InitData(); }
//        else { LoadCashedData();  }
//    }

//    void InitData()
//    {
//        // 임의로 생성
//        itemData = new ItemData();
//    }
//    void LoadCashedData()
//    {
//        itemData = LoadDataFromJson<ItemData>("ItemData");
//    }

//    // 데이터 저장하기
//    [ContextMenu("To Json Data")]
//    public void SaveDataToJson<T>(T data, string fileName ="")
//    {
//        string jsonData = JsonUtility.ToJson(data, true);
//        string filePath = string.IsNullOrEmpty(fileName) ? dataPath : Path.Combine(Application.persistentDataPath, fileName + ".json");
//        File.WriteAllText(dataPath, jsonData);

//        // 아이템 데이터 
//        if (typeof(T) == typeof(ItemData))
//        {
//            itemData = data as ItemData;
//        }
//        // 데이터 추가 시 여기에 추가
//    }

//    // 데이터 불러오기
//    [ContextMenu("From Json Data")]
//    public T LoadDataFromJson<T>(string fileName = "")
//    {
//        string filePath = string.IsNullOrEmpty(fileName)? dataPath : Path.Combine(Application.persistentDataPath, fileName + ".json");
//        // 저장된 파일이 있을 때
//        if (File.Exists(filePath)) 
//        {
//            string jsonData = File.ReadAllText(filePath);
//            T getData =  JsonUtility.FromJson<T>(jsonData);

//            // 아이콘,프리팹 로드
//            // 여기서 데이터 종류 추가
//            if (getData is ItemDatas itemDatas)
//            {
//                foreach (var itemData in itemDatas.itemDatas)
//                {
//                    itemData.icon = UnityEngine.Resources.Load<Sprite>(itemData.iconName);
//                    itemData.drobPrefab = UnityEngine.Resources.Load<GameObject>(itemData.drobPrefabName);

//                    if (itemData.icon == null)
//                        Debug.LogWarning($"아이콘 로드 안됨 {itemData.iconName}");
//                    if (itemData.drobPrefab == null)
//                        Debug.LogWarning($"프리팹 로드 안됨 {itemData.drobPrefabName}");
//                }
//            }
//            return getData;
//        }
//        // 저장된 파일이 없을 때
//        else
//        {
//            return default(T);
//        }
//    }

//    // 아이템 데이터 접근자 메소드
//    // 데이터 추가 시 여기에 추가
//    private class ItemWrapper<T>
//    {
//        public T data;
//    }

//}

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

        dataPath = Path.Combine(Application.persistentDataPath, "ItemData.Json");
        Debug.Log($"datapath는 {dataPath}");

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
            }
            Debug.Log($"Loaded JSON: {json}");
        }

    }

    public ItemData GetItemDataByID(string id)
    {
        if (itemDatas == null || itemDatas.itemsDatas == null)
        {
            Debug.LogWarning("아이템 데이터 없음");
            return null;
        }

        foreach (var item in itemDatas.itemsDatas)
        {
            if (item.itemKey == id)
            {
                return item;
            }
        }

        Debug.LogWarning($"{id} 아이템 데이터 없음.");
        return null;
    }
}