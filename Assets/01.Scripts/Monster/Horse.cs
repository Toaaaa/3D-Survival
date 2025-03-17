using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Horse : Monster
{
    [SerializeField] float randomRange = 33; // 랜덤 이동 반경 == navmeshsurface의 크기.
    int areaMask;
    private void Start()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        areaMask = 1 << NavMesh.GetAreaFromName("Horse");// Horse 영역만 검색.

        StartCoroutine(StateMachine());// 몬스터 상태 머신 시작.
    }
    private new void Update()
    {
        HpStatus();
    }
    protected override void HpStatus()
    {
        if (hp <= 0)
        {
            if (isDead == false)
            {
                isDead = true;
                DropItem();
                gameObject.SetActive(false);
            }
        }
        if (hpBar != null)
        {
            hpBar.sizeDelta = new Vector2(hp / maxHp * 2, hpBar.sizeDelta.y);
        }
    }
    protected override IEnumerator Idle()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (!curAnimStateInfo.IsName("Idle"))
            anim.Play("Idle", 0, 0);

        // Idle 상태에서는 Wander 코루틴 실행
        StartCoroutine(Wander());

        while (state == State.Idle)
            yield return null;
    }

    IEnumerator Wander()
    {
        while (state == State.Idle) // Idle 상태에서만 Wander를 한번 실행.
        {
            Vector3 randomPos = originPos + new Vector3(Random.Range(-randomRange,randomRange), 0, Random.Range(-randomRange, randomRange));

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 5f, areaMask))
            {
                navMeshAgent.SetDestination(hit.position);
                anim.Play("Walk"); // 걷기 애니메이션 재생

                //목표 지점에 도착할 때까지 대기
                while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                {
                    yield return null;
                }
                anim.Play("Idle"); // 도착 후 Idle 애니메이션 재생
            }

            yield return new WaitForSeconds(Random.Range(3f, 6f)); // 실행 후 대기 시간
        }
    }
}
