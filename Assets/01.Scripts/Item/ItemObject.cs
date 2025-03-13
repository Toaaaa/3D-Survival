using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering;

public interface IInteractable // 인터페이스
{
    public void OnInteract(); // 상호작용
    public string GetInteractPrompt(); // 프롬프트
}
public class ItemObject : MonoBehaviour, IInteractable
{
    //public static ItemObject instance;
    public ItemData itemData;

    //DataManager dataManager;

    //public ItemDatas objectItemData;
    //public ItemDatas GetItemDatas;

    //private string path;
    //private string fileName = "";

    //void Start()
    //{
    //    //instance = this;
    //    fileName = gameObject.name +".json";

       
    //    path = Path.Combine(Application.persistentDataPath, fileName);
    //    Debug.Log(path); // 경로확인용
    //    LoadItemDataFromJson();
    //}


    //[ContextMenu("To Json Data")]
    //public void SaveItemDataToJson()
    //{
    //    //string jsonData = JsonUtility.ToJson(itemData, true);
    //    string jsonData = JsonUtility.ToJson(objectItemData, true);
    //    File.WriteAllText(path, jsonData);            
    //}

    //[ContextMenu("From Json Data")]
    //public void LoadItemDataFromJson() 
    //{
    //    if (!File.Exists(path)) // 파일에 경로가 없다면 저장하고 불러온다
    //    {
    //        SaveItemDataToJson();
    //    }
        
    //    string JsonData = File.ReadAllText(path);
    //    //itemData = JsonUtility.FromJson<ItemData>(JsonData); 
    //    GetItemDatas = JsonUtility.FromJson<ItemDatas>(JsonData);
    //    LoadUnityObject();

    //}

    //void LoadUnityObject()
    //{
    //    for (int i = 0; i < GetItemDatas.itemDatas.Length; i++)
    //    {
    //        GetItemDatas.itemDatas[i].icon = UnityEngine.Resources.Load<Sprite>(GetItemDatas.itemDatas[i].iconName);
    //        GetItemDatas.itemDatas[i].drobPrefab = UnityEngine.Resources.Load<GameObject>(GetItemDatas.itemDatas[i].drobPrefabName);
    //    }
    //} 
    

    public string GetInteractPrompt()
    {
        //string str = $"{itemData.displayName}\n{itemData.description}";
        string str = ""; // 수정예정
        return str;
    }
    public virtual void OnInteract()
    {
        //아이템데이터에 넣어줌
        //CharacterManager.Instance.Player.ItemData = itemData;
        //액션
        //CharacterManager.Instance.Player.AddItem.Invoke();

        var playerInventory = CharacterManager.Instance.Player.inventory;

        if (playerInventory != null)
        {
            playerInventory.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
