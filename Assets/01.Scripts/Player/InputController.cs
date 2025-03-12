using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputController : MonoBehaviour
{
    Vector2 curMoveInput;
    public Vector2 CurMoveInput { get => curMoveInput; }
    Vector2 curMouseDelta;
    public Vector2 CurMouseDelta { get => curMouseDelta; }

    public Action jumpAction;

    PlayerInput input;

    private void OnEnable()
    {
        input = new PlayerInput();

        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMoveStop;
        input.Player.Jump.started += OnJump;

        input.Enable();
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMoveStop;
        input.Player.Jump.started -= OnJump;

        input.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        curMoveInput = context.ReadValue<Vector2>();
    }

    public void OnMoveStop(InputAction.CallbackContext context)
    {
        curMoveInput = Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpAction?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        curMouseDelta = context.ReadValue<Vector2>();
    }
}
