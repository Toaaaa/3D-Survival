using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlots : MonoBehaviour // UI아이템슬롯
{
    public ItemData ItemData;
    public UIInventory uiInventory;

    // 슬롯 구성요소
    public Button button; // 슬롯 칸
    public Image icon; //아이템 이미지
    public TextMeshProUGUI quantityText; // 수량 텍스트
    private Outline outline;

    // 슬롯 변수
    public int Index; // 슬롯 칸 index
    public bool equipped; // 착용여부
    public int quantity; // 슬롯 안의 아이템 개수

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }
    private void OnEnable()
    {
        outline.enabled = equipped; // equipped가 true일 때 ( = 착용했을 때) 아웃라인 활성
    }

    // 슬롯 선택(버튼클릭) -> UI 인벤토리 연결 메서드
    public void OnClick()
    {
        uiInventory.SelectItem(Index);
    }

    // 슬롯 정렬 메서드

    // 아이템이 들어왔을 때
    //public void Set(int index)
    //{
    //    icon.gameObject.SetActive(true);
    //    icon.sprite = uiInventory.playerInventory.slots[index].ItemData.icon;
    //    quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // 아이템 수량 없으면 표시 안 함

    //    // 방어코드
    //    if(outline != null ) {outline.enabled = equipped;}
    //}

    //// 아이템이 빠질 때
    //public void Clear()
    //{
    //    //uiInventory.playerInventory.slots[index].ItemData = null;
    //    icon.gameObject.SetActive(false);
    //    quantityText.text = string.Empty;
    //}
}
