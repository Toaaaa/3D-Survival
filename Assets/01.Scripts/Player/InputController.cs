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

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMoveInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMoveInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            jumpAction?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        curMouseDelta = context.ReadValue<Vector2>();
    }
}
