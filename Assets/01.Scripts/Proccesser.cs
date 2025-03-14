using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proccesser : MonoBehaviour, IInteractable
{
    public Canvas uiProccesser;

    private void Start()
    {
        //uiProccesser.gameObject.SetActive(false);
    }

    public string GetInteractPrompt()
    {
        string str = "자원 가공대";
        return str;
    }

    public void OnInteract()
    {
        StartCoroutine(OnProccesserCo());
    }

    private IEnumerator OnProccesserCo()
    {
        OnProccesserWindow();
        yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Escape));
        OffProccesserWindow();
    }

    private void OnProccesserWindow()
    {
        //플레이어 이동 및 둘러보기 잠금
        //마우스 커서 켜기
        //프로세서 창 켜기
        uiProccesser.gameObject.SetActive(true);
    }

    private void OffProccesserWindow()
    {
        //플레이어 이동 및 둘러보기 잠금 해제
        //마우스 커서 끄기
        //프로세서 창 끄기
        uiProccesser.gameObject.SetActive(false);
    }

}
