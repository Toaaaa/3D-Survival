using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public ItemData itemData;
    public ItemData[] itemDatas;
    // 다른 데이터 클래스 추가하면 여기에 참조

    private string dataPath; // 데이터 경로

    

    // 싱글톤
    private static DataManager instance;
    public static DataManager Instance
    {
        get 
        { 
            if (instance == null)
            {
                instance = new GameObject("DataManager").GetComponent<DataManager>();
            }
            return instance; 
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dataPath = Path.Combine(Application.persistentDataPath + "/Data", "/data.json");
        Debug.Log(dataPath); // 경로 확인용

        // 파일이 없을 때 임시 파일
        if (!File.Exists(dataPath)) { InitData(); }
        else { LoadCashedData();  }
    }

    void InitData()
    {
        // 임의로 생성
        itemData = new ItemData();
    }
    void LoadCashedData()
    {
        itemData = LoadDataFromJson<ItemData>("ItemData");
    }

    // 데이터 저장하기
    [ContextMenu("To Json Data")]
    public void SaveDataToJson<T>(T data, string fileName ="")
    {
        string jsonData = JsonUtility.ToJson(data, true);
        string filePath = string.IsNullOrEmpty(fileName) ? dataPath : Path.Combine(Application.persistentDataPath, fileName + ".json");
        File.WriteAllText(dataPath, jsonData);

        // 아이템 데이터 
        if (typeof(T) == typeof(ItemData))
        {
            itemData = data as ItemData;
        }
        // 데이터 추가 시 이곳에 추가
    }

    // 데이터 불러오기
    [ContextMenu("From Json Data")]
    public T LoadDataFromJson<T>(string fileName = "")
    {
        string filePath = string.IsNullOrEmpty(fileName)? dataPath : Path.Combine(Application.persistentDataPath, fileName + ".json");
        // 저장된 파일이 있을 때
        if (File.Exists(filePath)) 
        {
            string jsonData = File.ReadAllText(filePath);
            T getData =  JsonUtility.FromJson<T>(jsonData);

            //if (getData is ItemDatas itemDatas)
            //{
            //    for (int i = 0; i < itemDatas.cou; i++)
            //    {
            //        getData
            //    }
            //}
            //여기서 데이터 종류 추가

            return getData;
            
        }
        // 저장된 파일이 없을 때
        else
        {
            return default(T);
        }
    }

    // 프리팹, 아이콘 가져오는 메소드
    void LoadUnityObject()
    {

    }

    // 아이템 데이터 접근자 메소드
    public ItemData getItemData()
    {
        return itemData;
    }

    // 데이터 추가 시 여기에 추가

   
}
