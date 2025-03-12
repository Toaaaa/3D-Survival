using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    InputController input;

    [Header("FPCamera")]
    [SerializeField] Camera fpCamera;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float maxLookUp;
    [SerializeField] float maxLookDown;

    [Header("TPCamera")]
    [SerializeField] Camera tpCamera;

    private float curLookUp;
    private float curLookRight;
    private bool isCursor = false;
    

    private void Start()
    {
        input = GetComponent<InputController>();
        Cursor.lockState = CursorLockMode.Locked;
        fpCamera.gameObject.SetActive(true);
        tpCamera.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
         FPLook();
    }

    private void CursorToggle()
    {
        isCursor = !isCursor;
        Cursor.lockState = isCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void ChangeCamera()
    {
        bool isFPCameraActive = fpCamera.gameObject.activeSelf;
        
        fpCamera.gameObject.SetActive(!isFPCameraActive);
        tpCamera.gameObject.SetActive(isFPCameraActive);

    }

    private void FPLook()
    {
        Vector2 mouseDelta = input.CurMouseDelta;

        Debug.Log($"{curLookUp}");
        curLookUp += mouseDelta.y * mouseSensitivity;
        curLookRight += mouseDelta.x * mouseSensitivity;

        curLookUp = Mathf.Clamp(curLookUp, maxLookDown, maxLookUp);

        fpCamera.transform.localRotation = Quaternion.Euler(-curLookUp, curLookRight, 0);
    }
}
