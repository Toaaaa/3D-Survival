using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackPower;
    public float AttackPower { get => attackPower; }
    [SerializeField] float attackDistance;
    public float AttackDistance { get => attackDistance; }
    public Action onHitMonster;
    public Action onHitResorce;

    //장비 착용시 가져다 쓰면 됩니다.
    public void ChangePower(float value)
    {
        if (attackPower <= 0) return;

        attackPower += value;
    }

    public void ChangeAttackDistance(float value)
    {
        if (attackDistance <= 0) return;
        attackDistance += value;
    }

    public void Attack()
    {
        if (!OnHit()) return;        
    }

    private bool OnHit()
    {
        //애니메이션 공격 이벤트 함수에 추가

        Ray ray = new Ray (transform.position + (Vector3.up * 0.5f), transform.forward);
        RaycastHit hit;
        Debug.Log("공격");
        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (hit.collider.TryGetComponent(out Resources resources))
            {
                Debug.Log("자원 공격 성공");
                resources.Gather(hit.point , hit.normal);
                onHitResorce?.Invoke();
                return true;
            }

            else if(hit.collider.TryGetComponent(out Monster monster))
            {
                Debug.Log("적 공격 성공");               
                monster.OnHit(attackPower);
                onHitMonster?.Invoke();
                return true;
            }    
        }
        return false;
    }
}
