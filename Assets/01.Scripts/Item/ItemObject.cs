using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering;
//using static UnityEditor.Progress;

public interface IInteractable // 인터페이스
{
    public void OnInteract(); // 상호작용
    public string GetInteractPrompt(); // 프롬프트
}
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    public string itemKey;
    public int amount;

    private void Start()
    {
        itemData = DataManager.Instance.GetItemDataByID(itemKey);      
    }

    public string GetInteractPrompt()
    {
        string str = $"{itemData.displayName}({amount})\n{itemData.description}";
        
        return str;
    }
    public virtual void OnInteract()
    {
       
        var playerInventory = CharacterManager.Instance.Player.inventory;

        if (playerInventory != null)
        {
            for(int i = 0; i< amount; i++)
            {
                playerInventory.AddItem(itemData);
            }
            
            Destroy(gameObject);
        }
    }
}
