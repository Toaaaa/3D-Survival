using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProccesser : MonoBehaviour
{
    public UIItemSlots[] slots;
    public Image ProccessingItemIcon;
    public Image ProccessedItemIcon;
    public TextMeshProUGUI neededQuantityTxt;
    public Button proccessBtn;


    public ItemData selectedItem;
    public int selectedItemIndex;

    private void Awake()
    {
        proccessBtn.onClick.AddListener(OnProccessBtn);
    }

    public void OnProccessBtn()
    {
        if(selectedItem != null && selectedItem.type == ItemType.Resource)//proccessedItem != null && quantity<neededQuantity
        {

        }
    }
}
