using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

public class Interaction : MonoBehaviour
{
    private float checkRate = 0.05f;
    private float lastCheckTime;
    private float maxCheckDistance = 2;
    private LayerMask layerMask;

    private GameObject curInteractGameObject;

    private Camera camera;
    private bool nowFirstPerson = true;

    private Transform interactionRayPointTransform;

    private void Start()
    {
        layerMask = 1 << 6;
        camera = Camera.main;
        lastCheckTime = Time.time;

        //구성을 단순화하기 위해 이렇게 초기화했습니다. GetChild를 활용해서 초기화하는 방법은 권장되지 않습니다.
        interactionRayPointTransform = transform.GetChild(0).GetChild(1);
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = returnInteractionRay();
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    Debug.Log($"{curInteractGameObject.name}과 상호작용할 수 있습니다.");
                }
            }
            else
            {
                curInteractGameObject = null;
            }
        }

        // 스페이스 바를 눌렀을 때 시점을 전환합니다.
        if (Input.GetKeyDown(KeyCode.Space)) SwitchView();
    }

    public void SwitchView()
    {
        if (nowFirstPerson)// 1인칭 에서 3인칭 전환
        {
            nowFirstPerson = false;
            camera.transform.localPosition = new Vector3(0, 0.5f, -5);
        }
        else//3인칭에서 1인칭 전환.
        {
            nowFirstPerson = true;
            camera.transform.localPosition = Vector3.zero;
        }
    }

    private Ray returnInteractionRay()
    {

        if (nowFirstPerson)//1인칭일때
        {
            //TODO
            //camera를 활용할 것
            return camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));// 화면 중앙에서 ray.
        }
        else//3인칭일때/
        {
            //TODO
            //interactionRayPointTransform를 활용할 것
            return new Ray(interactionRayPointTransform.position, interactionRayPointTransform.forward);
        }
    }
}
