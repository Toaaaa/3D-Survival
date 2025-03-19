using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnvironmentCon : MonoBehaviour
{
    public EnvironmentType environmentType;

    //툰드라 효과
    bool isFreezing = false;
    public float freezeTime = 5f;
    [SerializeField] Image freezeOverlay;// 화면 얼어붙는 효과.

    private void Start()
    {
        StartCoroutine(CheckEnvironment());
    }
    void Update()
    {
        if (environmentType == EnvironmentType.Tundra)
        {
            StartCoroutine(ScreenFreeze());
        }
        else
        {
            StartCoroutine(ScreenMelt());
        }
    }

    void EnvironmentEffect()
    {
        if(environmentType == EnvironmentType.Tundra || environmentType == EnvironmentType.Desert)
        {
            Player player = GetComponent<Player>();
            player.condition.Conditions[ConditionType.Health].curValue -= 5;// 툰드라/사막 환경에 있을 때 매 시간마다 5씩 체력 감소.
        }
    }
    IEnumerator CheckEnvironment()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);// 3초마다 환경 체크 + 환경에 따른 효과 적용.
            EnvironmentEffect();
        }
    }

    IEnumerator ScreenFreeze()// 화면 얼리는 효과
    {
        if (isFreezing) yield break; // 중복 실행 방지
        isFreezing = true;

        freezeOverlay.gameObject.SetActive(true);// 오버레이 활성화
        float elapsedTime = 0f;
        while (elapsedTime <= freezeTime)
        {
            freezeOverlay.color = new Color(0, 0.5f, 1f, elapsedTime / freezeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }       
    }

    IEnumerator ScreenMelt()// 화면 녹는 효과
    {
        if (!isFreezing) yield break; // 중복 실행 방지

        float elapsedTime = 0f;
        while (elapsedTime <= freezeTime)
        {
            freezeOverlay.color = new Color(0, 0.5f, 1f, 1 - elapsedTime / freezeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        freezeOverlay.gameObject.SetActive(false);// 오버레이 비활성화
        isFreezing = false;
    }
}
