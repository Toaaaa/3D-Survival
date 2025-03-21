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
        InvokeRepeating("CheckInterable", 0, 0.1f);
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

    public bool OnIntercat()
    {        
        if (curIteractGameObject == null) return false;

        if (curIteractGameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (CharacterManager.Instance.Player.condition.Conditions.TryGetValue(ConditionType.Water, out Condition water))
            {
                water.ChangCondition(water.MaxValue);
            }
            return true;
        }

        if (curItercable == null) return false;

        curItercable.OnInteract();
        curItercable = null;
        curIteractGameObject = null;
        promptItemInfo.text = null;
        return true;
    }
}
