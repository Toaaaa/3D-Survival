using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputController : MonoBehaviour
{
    Vector3 curMoveInput;  
    public Vector3 CurMoveInput { get => curMoveInput; }   

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("키입력 받았다");
        if (context.phase == InputActionPhase.Performed)
        {
            curMoveInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMoveInput = Vector2.zero;
        }
    }
}
