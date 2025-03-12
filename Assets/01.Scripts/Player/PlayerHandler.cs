using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    public float Speed { get => speed; }
    [SerializeField] private float jumpPower;
    public float JumpPower { get => jumpPower; }

    InputController input;
    Rigidbody rigid;    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        input = GetComponent<InputController>();
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
}