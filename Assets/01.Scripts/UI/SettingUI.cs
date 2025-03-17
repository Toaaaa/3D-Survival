using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    [SerializeField] GameObject settingUI ;
    CameraController cameraCont;
    UIInventory uiInventory;

    private void Start()
    {
        cameraCont = Camera.main.GetComponent<CameraController>();
        uiInventory = FindObjectOfType<UIInventory>();
    }

    public void OnClick()
    {
        if (settingUI.activeSelf)//설정이 켜져있는 상태.
        {
            cameraCont.CursorToggle();
            settingUI.SetActive(false);
        }
        else//설정이 꺼져있는 상태.
        {
            //인벤 UI 끄기.
            uiInventory.inventoryWindow.SetActive(false);
            settingUI.SetActive(true);
        }
    }
}
