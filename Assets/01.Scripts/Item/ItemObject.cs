using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable // 인터페이스
{
    //public void OnInteract(); // 상호작용
    //public string GetInteractPrompt(); // 프롬프트
}
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData ItemData;

    //public string GetInteractPrompt()
    //{
        
    //}
    //public void OnInteract()
    //{
        
    //}
}
