using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [Header("Monster Status")]
    public string monsterName;
    [SerializeField] protected bool isDead = false;
    [SerializeField] protected float maxHp = 40;
    [SerializeField] private float _hp;
    public float hp
    {
        get => _hp;
        private set => _hp = value;
    }
    [SerializeField] private int _attackPower;
    public int attackPower
    {
        get => _attackPower;
        set => _attackPower = value;
    }

    [Header("HP Bar")]
    [SerializeField] protected RectTransform hpBar;// 체력이 최대일때는 Width가 2.

    [Header("Monster AI")]
    [SerializeField] private Transform target;
    [SerializeField] private float lostDistance = 13; // 타겟을 잃어버리는 거리.
    [SerializeField] private float AwayDistance = 33; // 처음 구역에서 최대로 멀어질 수 있는 거리. (해당 거리 이상으로 떨어지면 모든 행동을 중지하고 원래 위치로 타겟을 설정하여 돌아감.)
    public Vector3 originPos { get; set; } // 처음 위치.
    protected NavMeshAgent navMeshAgent;
    protected Animator anim;
    protected State state = State.Idle;
    protected enum State
    {
        Idle,
        Chase,
        Attack,
        Return, // 원래 위치로 돌아가는 상태.
        Dead
    }// 몬스터 상태
    public Func<bool> InRange;


    private void OnEnable()
    {
        isDead = false;
        _hp = maxHp;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = CharacterManager.Instance.Player.transform;
        originPos = transform.position;// 처음 위치 저장.
        StartCoroutine(StateMachine());// 몬스터 상태 머신 시작.
    }
    protected virtual void Update()
    {
        HpStatus();// 체력 상태 관리 전반.

        // 몬스터 AI
        if (isDead) return;
        if(Vector3.Distance(originPos,this.transform.position) >= AwayDistance || state == State.Return)// 만약 처음 위치에서 awayDistance 이상 떨어지면 원래 구역으로 우선적으로 복귀.
        {
            target = null;
            ChangeState(State.Return); ;
            navMeshAgent.SetDestination(originPos);
            return;
        }
        if (InRange() && target == null)// 범위 내에 플레이어가 있고, 타겟이 없을 때
        {
            target = CharacterManager.Instance.Player.transform;
            ChangeState(State.Chase);
        }
        else if (!InRange())
        {
            target = null;
            ChangeState(State.Idle);
        }
        if (target == null) return; // 타겟이 없거나, 범위 밖일때는 return.
        navMeshAgent.SetDestination(target.position);
    }

    protected virtual void HpStatus()
    {
        
    }// 체력관련 메서드.
    protected void DropItem()
    {
        MonsterManager.Instance.monsterDropManager.DropItem(monsterName,this.transform.position);
    }// 처치시 아이템 드랍.
    protected void ChangeState(State newState)
    {
        state = newState;
    }
    public void OnHit(float dmg) // 피격시 호출되는 메서드.
    {
        hp -= dmg;
    }
    protected virtual float GetHpPercent()
    {
        return hp / maxHp;
    }

    protected virtual IEnumerator StateMachine()
    {
        while(true)
            yield return StartCoroutine(state.ToString());
    }
    protected virtual IEnumerator Idle()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnimStateInfo.IsName("Idle") == false)
            anim.Play("Idle", 0, 0);

        while(state == State.Idle)
            yield return null;
    }

    protected virtual IEnumerator Chase()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnimStateInfo.IsName("Chase") == false)
        {
            anim.Play("Chase", 0, 0);
            yield return null;// SetDestination 을 위해 frame.
        }

        // 목표까지의 남은 거리가 멈추는 지점보다 작거나 같으면 StateMachine 을 공격으로 변경
        while (state == State.Chase)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                ChangeState(State.Attack);
                yield break;
            }
            else if (navMeshAgent.remainingDistance > lostDistance)
            {
                target = null;
                ChangeState(State.Idle);
                yield break;
            }
            yield return null; // 한 프레임마다 체크
        }
    }
    protected virtual IEnumerator Attack()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        anim.Play("Attack", 0, 0);
        if (target != null) 
        {
            target.GetComponent<Player>().condition.Conditions[ConditionType.Health].curValue -= attackPower;// 공격력만큼 플레이어에게 데미지를 줌.
        }
        // 거리가 멀어지면
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            // StateMachine을 추적으로 변경
            ChangeState(State.Chase);
        }
        else
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance - 1f) yield return new WaitForSeconds(curAnimStateInfo.length * 2f);// 만약 공격 중(2번째 타격 시전전) 에 플레이어가 범위 밖으로 나가면 공격 취소
            else
            {                
                if (!curAnimStateInfo.IsName("Attack_Idle") && target != null)
                    target.GetComponent<Player>().condition.Conditions[ConditionType.Health].curValue -= attackPower;// 공격력만큼 플레이어에게 데미지를 줌.   

                yield return new WaitForSeconds(curAnimStateInfo.length * 2f);// 공격 animation 의 두 배만큼 대기
            }
        }
    }
    protected virtual IEnumerator Return()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnimStateInfo.IsName("Chase") == false)
        {
            anim.Play("Chase", 0, 0);
            yield return null;// SetDestination 을 위해 frame.
        }

        // 목표까지의 남은 거리가 멈추는 지점보다 작거나 같으면 StateMachine 을 공격으로 변경
        if (Vector3.Distance(originPos, this.transform.position) <= 3.5f)// 원래 위치에 근접하면 Return 상태 종료.
        {
            ChangeState(State.Idle);
            yield break;
        }
    }
}
