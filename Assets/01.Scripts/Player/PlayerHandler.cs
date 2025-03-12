using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    public float Speed { get => speed; }
    [SerializeField] private float jumpPower;
    public float JumpPower { get => jumpPower; }

    [Header("Look")]
    private bool isFP;

    InputController input;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        input = GetComponent<InputController>();
    }

    private void Start()
    {
        input.jumpAction += Jump;
    }

    private void FixedUpdate()
    {
        Move(input.CurMoveInput);
    }

    private void Move(Vector2 input)
    {
        
        Vector3 moveDir = transform.forward * input.y + transform.right * input.x;
        moveDir *= speed;

        moveDir.y = rigid.velocity.y;

        rigid.velocity = moveDir;
    }

    private void Jump()
    {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
    }

    private void Look()
    {
        
    }
}