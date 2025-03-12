using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable // 인터페이스
{
    public void OnInteract(); // 상호작용
    public string GetInteractPrompt(); // 프롬프트
}
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    public string GetInteractPrompt()
    {
        string str = $"{itemData.displayName}\n{itemData.description}";

        return str;
    }
    public void OnInteract()
    {
        //아이템데이터에 넣어줌
        //CharacterManager.Instance.Player.ItemData = itemData;
        //액션
        //CharacterManager.Instance.Player.AddItem.Invoke();
        
        Destroy(gameObject);
    }
}
