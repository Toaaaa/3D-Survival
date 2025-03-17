using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Action runAction;
    public Action inventory;

    PlayerInput input;

    private void OnEnable()
    {
        input = new PlayerInput();

        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMoveStop;
        input.Player.Jump.started += OnJump;
        input.Player.Run.performed += OnRun;
        input.Player.Run.canceled += OnRunStop;      
        //input.Player.CameraChange.started += OnCameraChange;
        input.Player.Look.performed += OnLook;
        input.Player.Look.canceled += OnLookCancle;
        input.Player.Gather.started += OnInterack;
        input.Player.Attack.started += OnAttack;
        input.Player.Inven.started += OnInven;
        input.Player.Build.started += OnBuild;

        input.Enable();
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMoveStop;
        input.Player.Jump.started -= OnJump;
        input.Player.Run.performed -= OnRun;
        input.Player.Run.canceled -= OnRunStop;    
        //input.Player.CameraChange.started -= OnCameraChange;
        input.Player.Look.performed -= OnLook;
        input.Player.Look.canceled -= OnLookCancle;
        input.Player.Gather.started -= OnInterack;
        input.Player.Attack.started -= OnAttack;
        input.Player.Inven.started -= OnInven;
        input.Player.Build.started -= OnBuild;

        input.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        curMoveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveStop(InputAction.CallbackContext context)
    {
        curMoveInput = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpAction?.Invoke();
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        CharacterManager.Instance.Player.handler.isRun = true;
        runAction?.Invoke();
    }

    private void OnRunStop(InputAction.CallbackContext context)
    {
        CharacterManager.Instance.Player.handler.isRun = false;
        StopCoroutine(CharacterManager.Instance.Player.handler.RunStaminaUsing());
    }
    private void OnLook(InputAction.CallbackContext context)
    {
        curMouseDelta = context.ReadValue<Vector2>();    
    }

    private void OnLookCancle(InputAction.CallbackContext context)
    {
        curMouseDelta = Vector2.zero;
    }

    //private void OnCameraChange(InputAction.CallbackContext context)
    //{
    //    GameObject cameraContainer = GameObject.Find("CameraContainer");
    //    CameraController controller = cameraContainer.GetComponent<CameraController>();
    //    controller.ChangeCamera();
    //}

    private void OnInterack(InputAction.CallbackContext context)
    {
        CharacterManager.Instance.Player.handler.Gather();
    }

    private void OnInven(InputAction.CallbackContext context)
    {
        //확인 필요 UIInventory 에 토글 로직 있고 Player 에는 PlayerInventory 붙여주셨음
        inventory?.Invoke();
    }

    private void OnBuild(InputAction.CallbackContext context)
    {
        BuildUI.Instance.radialMenu.Toggle();
        if(CharacterManager.Instance.Player.building.runBuilding)
            CharacterManager.Instance.Player.building.ClearPreview();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        CharacterManager.Instance.Player.handler.Attack();
    }
}
