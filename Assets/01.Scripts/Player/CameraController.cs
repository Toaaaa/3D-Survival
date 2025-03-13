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
    [HideInInspector]public bool isFPCamear= true;

    private void Start()
    {
        input = GetComponentInParent<InputController>();
        Cursor.lockState = CursorLockMode.Locked;
        fpCamera.gameObject.SetActive(true);
        tpCamera.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (isCursor) return;
        
        FPLook();
        
    }

    public void ChangeCamera()
    {
        bool isFPCameraActive = fpCamera.gameObject.activeSelf;

        isFPCamear = !isFPCamear;
        fpCamera.gameObject.SetActive(!isFPCameraActive);
        tpCamera.gameObject.SetActive(isFPCameraActive);

    }

    private void FPLook()
    {
        Vector2 mouseDelta = input.CurMouseDelta;

        curLookUp += mouseDelta.y * mouseSensitivity;
        curLookRight += mouseDelta.x * mouseSensitivity;

        curLookUp = Mathf.Clamp(curLookUp, maxLookDown, maxLookUp);

        transform.root.localRotation = Quaternion.Euler(-curLookUp, curLookRight, 0);
    }

    private void TPLook()
    {

    }

    private void CursorToggle()
    {
        isCursor = !isCursor;
        Cursor.lockState = isCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
