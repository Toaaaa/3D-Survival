using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interanction : MonoBehaviour
{
    public GameObject curIteractGameObject;
    IInteractable curItercable;

    [SerializeField] float maxCheckDistance;
    [SerializeField] LayerMask intercatLayer;
    [SerializeField] TextMeshProUGUI promptItemInfo;

    CameraController controller;
    Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
        controller = GetComponentInChildren<CameraController>();
    }

    private void Update()
    {
        if (controller.isFPCamear)
        {
            InvokeRepeating("CheckInterable", 0, 0.1f);
        }
        
    }

    private void CheckInterable()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2 , Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, intercatLayer ))
        {
            curIteractGameObject = hit.collider.gameObject;
            curItercable = hit.collider.GetComponent<IInteractable>();
            SetPrompText();
        }
        else
        {
            curIteractGameObject = null;
            curItercable = null;
            promptItemInfo.gameObject.SetActive(false);
        }
    }
    private void SetPrompText()
    {
        if (curItercable == null) return;

        promptItemInfo.gameObject.SetActive(true);
        promptItemInfo.text = curItercable.GetInteractPrompt();
        
    }

    private void OnIntercat()
    {
        if (curIteractGameObject = null) return;
        CharacterManager.Instance.Player.handler.Gather();
        curItercable.OnInteract();
    }
}
