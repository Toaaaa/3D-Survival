using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickMove : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;

    [SerializeField] Transform point;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        anim.SetBool("IsJump", agent.isOnOffMeshLink);
        anim.SetBool("IsGroud", !agent.isOnOffMeshLink);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                anim.SetBool("IsMove", true);

                point.gameObject.SetActive(true);
                point.position = hit.point; // 클릭한 위치에 포인트 이동
            }
        }

        else if (agent.remainingDistance < 0.1f)
        {
            anim.SetBool("IsMove", false);
            anim.SetBool("IsGroud", true); // 이동이 끝나면 IsGround를 true로 설정
            point.gameObject.SetActive(false); // 이동이 끝나면 포인트 비활성화
        }
    }
}
