using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerHandler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float freezingSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float runStamina;
    [SerializeField] private LayerMask groundLayer;

    [HideInInspector] public bool isRun = false;
    [HideInInspector] public bool isWeopon = false;

    PlayerAnimator animator;
    InputController input;
    Rigidbody rigid;
    CameraController cameraController;

    Coroutine runCoroutine;

    private void Start()
    {
        animator = GetComponent<PlayerAnimator>();
        rigid = GetComponent<Rigidbody>();
        cameraController = GetComponentInChildren<CameraController>();
        input = GetComponent<InputController>();
        input.jumpAction += Jump;
        input.runAction += Run;
    }

    private void FixedUpdate()
    {

        if (CharacterManager.Instance.Player.isDead) return;

        if (CharacterManager.Instance.Player.condition.isFreezing)
        {
            Move(freezingSpeed);
        }
        else if (isRun)
        {
            Move(runSpeed);
        }
        else
        {
            Move(walkSpeed);
        }

        animator.IsGrouded(IsGrounded());
    }

    private void Move(float speed)
    {
        Vector2 _inputVector = input.CurMoveInput;
        Vector3 moveDir = transform.forward * _inputVector.y + transform.right * _inputVector.x;
        moveDir *= speed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
        
        animator.Run(isRun);
        animator.Move(_inputVector);
    }

    private void Jump()
    {
        if (!IsGrounded() || CharacterManager.Instance.Player.isDead) return;

        rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        animator.Jump();
        animator.IsGrouded(IsGrounded());
    }

    public void Run()
    {
        runCoroutine = StartCoroutine(RunStaminaUsing());
    }

    public IEnumerator RunStaminaUsing()
    {
        while (isRun)
        {
            if(!CharacterManager.Instance.Player.condition.UseStamina(runStamina * Time.deltaTime))
            {
                //사용 가능스태미나 없을시 종료
                if (!CharacterManager.Instance.Player.condition.UseStamina(runStamina))
                {
                    isRun = false;
                    yield break;
                }
            }
            yield return null;
        }      
    }

    public void Gather()
    {
        if (!CharacterManager.Instance.Player.interact.OnIntercat()) return;
        animator.Gather();
    }

    //지면 확인
    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.3f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.3f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (transform.right* 0.3f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (-transform.right * 0.3f) + (transform.up * 0.01f),Vector3.down)
        };
        RaycastHit hit;

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], out hit, 0.1f, groundLayer))
            {
                PlayerEnvironmentCon envCon = GetComponent<PlayerEnvironmentCon>();
                envCon.environmentType = hit.transform.GetComponent<Environment>().environmentType;// 플레이어가 밟고 있는 땅의 환경을 저장.
                return true;
            }
        }
         
        return false;
    }

    public void Attack()
    {
        //무기 장착 여부 관련해서 에니메이터 재생v
        if (!cameraController.isLook) return;
        if (isWeopon)
        {
            animator.AttackWeopon();
        }
        else
        {
            animator.AttackPuch();
        }        
    }

}