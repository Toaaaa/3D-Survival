using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProcessItemSlot : MonoBehaviour
{
    public int Index; // 슬롯 칸 index
    private UIProccesser proccesser;

    public Button button; // 슬롯 칸
    public Image icon; //아이템 이미지
    public TextMeshProUGUI quantityText; // 수량 텍스트

    private void Awake()
    {
        button = GetComponent<Button>();
        proccesser = GetComponentInParent<UIProccesser>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        proccesser.Select(Index);
    }
}
