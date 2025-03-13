using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proccesser : MonoBehaviour, IInteractable
{
    //public Inventory inven;
    //public UIProccesser uiProccesser;
    private void Start()
    {
        //inven = CharacterManager.Player.inventory;
    }

    public string GetInteractPrompt()
    {
        string str = "자원 가공대";
        return str;
    }

    public void OnInteract()
    {

    }

    private IEnumerator OnProccesserCo()
    {
        //inven.Toggle();
        yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Escape));
        //inven.Toggle();
    }

    private void OnProccesserWindow()
    {
        //플레이어 이동 및 둘러보기 잠금
        //마우스 커서 켜기
        //프로세서 창 켜기
    }

}
