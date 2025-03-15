using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    static readonly int IsMove = Animator.StringToHash("IsMove");
    static readonly int IsRun = Animator.StringToHash("IsRun");
    static readonly int IsJump = Animator.StringToHash("IsJump");
    static readonly int IsGroud = Animator.StringToHash("IsGroud");
    static readonly int IsGather = Animator.StringToHash("IsGather");
    static readonly int IsPunch = Animator.StringToHash("IsPunch");
    static readonly int IsWeopon = Animator.StringToHash("IsWeopon");

    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMove, obj.magnitude > 0.1f );
    }

    public void Run(bool obj)
    {
        animator.SetBool(IsRun, obj);
    }

    public void Jump()
    {
        animator.SetTrigger(IsJump);
    }

    public void IsGrouded(bool isGround)
    {
        animator.SetBool(IsGroud, isGround);
    }

    public void Gather()
    {
        animator.SetTrigger(IsGather);
    }

    public void AttackPuch()
    {
        animator.SetTrigger(IsPunch);
    }

    public void AttackWeopon()
    {
        animator.SetTrigger(IsWeopon);
    }
}
