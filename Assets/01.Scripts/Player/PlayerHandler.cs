using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float freezingSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float runStamina;
    [HideInInspector]public bool isRun = false;

    [Header("Look")]
    private bool isFP;

    InputController input;
    Rigidbody rigid;

    Coroutine runCoroutine;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        input = GetComponent<InputController>();
    }

    private void Start()
    {
        input.jumpAction += Jump;
        input.runAction += Run;
    }

    private void FixedUpdate()
    {
        if (CharacterManager.Instance.Player.condition.isFreezing)
        {
            Move(freezingSpeed);
        }
        else if(isRun)
        {
            Move(runSpeed);
        }
        else Move(walkSpeed);

    }

    private void Move(float speed)
    {
        Vector2 _inputVector = input.CurMoveInput;
        Vector3 moveDir = transform.forward * _inputVector.y + transform.right * _inputVector.x;
        moveDir *= speed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }

    private void Jump()
    {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
    }

    public void Run()
    {
        CharacterManager.Instance.Player.condition.UseStamina(runStamina);
        runCoroutine = StartCoroutine(RunStaminaUsing());
    }

    IEnumerator RunStaminaUsing()
    {
        while (isRun)
        {
            CharacterManager.Instance.Player.condition.UseStamina(runStamina * Time.deltaTime);         
        }
        yield return null;
    }
}