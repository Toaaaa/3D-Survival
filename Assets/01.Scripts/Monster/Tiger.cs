using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tiger : Monster
{
    bool isAlter = false;// 일정시간 유지되는 분신. (일정 수치 이하로 체력이 떨어지면 분신을 소환한다.)
    bool isCoolTime = false;
    float alterTime = 20f;// 본체 : 분신 소환 쿨타임. 분신 : /2 == 유지 시간.
    Coroutine coroutine;
    [SerializeField] GameObject alter;

    protected override void Update()
    {
        base.Update();
        if(GetHpPercent() <= 0.5f)
        {
            if (!isAlter && !isCoolTime)
            {
                coroutine = StartCoroutine(Resummon());
            }
        }
        if(isAlter)
            coroutine = StartCoroutine(AlterTime());
    }
    private void OnDestroy()
    {
        if (isAlter)
        {
            StopCoroutine(coroutine);// 파괴시 실행중인 코루틴 취소.
        }
    }
    protected override void HpStatus()
    {
        if (hp <= 0)
        {
            if (isDead == false)
            {
                isDead = true;
                DropItem();
                //gameObject.SetActive(false); << 공통으로 사용하는 부분
                if(!isAlter)
                    gameObject.SetActive(false);// 분신이 아닐때만 비활성화.
                else
                    Destroy(gameObject);// 분신일때는 파괴.
                ChangeState(State.Dead);
            }
        }
        if (hpBar != null)
        {
            hpBar.sizeDelta = new Vector2(hp / maxHp * 2, hpBar.sizeDelta.y);
        }
    }
    void SummonAlter()
    {
        if (isAlter) return;
        GameObject alt = Instantiate(alter,transform.position + Vector3.left * 1.5f, Quaternion.identity); // 분신을 왼쪽에 소환.
        Tiger a = alt.GetComponent<Tiger>();
        a.isAlter = true;
        a.originPos = this.originPos + Vector3.left *2f;
    }
    IEnumerator Resummon()// 분신의 재소환 쿨타임.
    {
        isCoolTime = true;
        SummonAlter();
        yield return new WaitForSeconds(alterTime);
        isCoolTime = false;
    }
    IEnumerator AlterTime()// 분신의 유지 시간.
    {
        yield return new WaitForSeconds(alterTime/2);
        Destroy(gameObject);
    }
}
