using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CameraController : MonoBehaviour
{
    InputController input;

    //[Header("FPCamera")]
    //[SerializeField] Camera fpCamera;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float maxLookUp;
    [SerializeField] float maxLookDown;

    [Header("TPCamera")]
    [SerializeField] Camera tpCamera;

    private Camera curCamera;
    private float curLookUp;
    private float curLookRight;
    public bool isLook = true;

    //[HideInInspector]public bool isFPCamear= true;
    public Camera CurCamera  { get { return curCamera; } set { curCamera = value; } }

    private void Start()
    {
        input = GetComponentInParent<InputController>();
        input.inventory += CursorToggle;
        Cursor.lockState = CursorLockMode.Locked;
        //fpCamera.gameObject.SetActive(true);
        //tpCamera.gameObject.SetActive(false);
        curCamera = tpCamera;
        BuildManager.Instance.cameraController = this;
    }

    private void LateUpdate()
    {
        if (!isLook || CharacterManager.Instance.Player.isDead) return;
        
        Look();
        
    }

    //public void ChangeCamera()
    //{
    //    bool isFPCameraActive = fpCamera.gameObject.activeSelf;
    
    //    isFPCamear = !isFPCamear;
    //    curCamera = isFPCamear ? fpCamera : tpCamera;s
    //    fpCamera.gameObject.SetActive(!isFPCameraActive);
    //    tpCamera.gameObject.SetActive(isFPCameraActive);

    //}

    private void Look()
    {
        Vector2 mouseDelta = input.CurMouseDelta;               
        
        curLookUp += mouseDelta.y * mouseSensitivity;
        curLookRight += mouseDelta.x * mouseSensitivity;
        curLookUp = Mathf.Clamp(curLookUp, maxLookDown, maxLookUp);

        

        //y축s
        float heightOffset = Mathf.Lerp(-1f, 4f, (-curLookUp - maxLookDown) / (maxLookUp - maxLookDown));
        //z축
        float distanceOffset = Mathf.Lerp(-2f, -1f, (-curLookUp - maxLookDown) / (maxLookUp - maxLookDown));

        

        Vector3 cameraTargetPos = tpCamera.transform.localPosition;
        cameraTargetPos.y = heightOffset;
        cameraTargetPos.z = distanceOffset;
        tpCamera.transform.localPosition = cameraTargetPos;

        curCamera.transform.localRotation = Quaternion.Euler(-curLookUp, 0, 0);
        transform.root.localRotation = Quaternion.Euler(0, curLookRight, 0);
    }

    public void CursorToggle()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ?  CursorLockMode.None : CursorLockMode.Locked;
        isLook = !toggle;
    }
}
