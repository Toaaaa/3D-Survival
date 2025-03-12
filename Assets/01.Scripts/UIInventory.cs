using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{    
    public GameObject inventoryWindow; // 인벤토리창
    public Transform slotPanel; // 슬롯
    public Transform dropPosition; // 버리기 위치


    private void Start()
    {
        inventoryWindow.SetActive(false); // 창 초기화
    }


    // 인벤토리 창 열고 닫기
    public void Toggle()
    {
        if (IsOpen())
        { inventoryWindow.SetActive(false); }
        else { inventoryWindow.SetActive(true); }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }
}
