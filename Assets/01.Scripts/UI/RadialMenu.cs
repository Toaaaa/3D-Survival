using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class RadialMenu : MonoBehaviour
{
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private float radius = 300f;
    [SerializeField] List<BuildList> buildList;

    [SerializeField] public TextMeshProUGUI nameObject;
    [SerializeField] public TextMeshProUGUI descriptionObject;
    
    private List<RadialMenuEntry> entries;
    CameraController cameraController;
    
    public Action openAction;

    private void Awake()
    {
        BuildUI.Instance.radialMenu = this;
    }

    private void Start()
    {
        entries = new List<RadialMenuEntry>();
        cameraController = FindAnyObjectByType<CameraController>();
        
        // cameraController.CursorToggle();
        
        // buildList의 아이콘, 빌드할 buildObject 프리팹 할당하고 disable
        for (int i = 0; i < buildList.Count; i++)
        {
            AddEntry(buildList[i].buildObject.icon, buildList[i].buildObject.prefab);
            entries[i].gameObject.SetActive(false);
        }
    }

    private void AddEntry(Texture pIcon,GameObject pPrefab)
    {
        GameObject entry = Instantiate(entryPrefab, transform);
        
        RadialMenuEntry rme = entry.GetComponent<RadialMenuEntry>();
        rme.SetPrefab(pPrefab);
        rme.SetIcon(pIcon);
        
        entries.Add(rme);
    }

    public void OpenMenu()
    {
        cameraController.CursorToggle();
        Cursor.lockState = CursorLockMode.None; // 토글 막혀서 일단 테스트
        for (int i = 0; i < buildList.Count; i++)
        {
            entries[i].gameObject.SetActive(true);
        }
        Rearrange();
        
        openAction?.Invoke();
    }

    public void CloseMenu()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        cameraController.CursorToggle();


        for (int i = 0; i < buildList.Count; i++)
        {
            RectTransform rect = entries[i].GetComponent<RectTransform>();
            GameObject entry = entries[i].gameObject;

            rect.DOAnchorPos(Vector3.zero, .3f).SetEase(Ease.OutQuad).onComplete
                = delegate()
                {
                    entry.SetActive(false);
                };
        }
        
        nameObject.gameObject.SetActive(false);
        descriptionObject.gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (!entries[0].gameObject.activeSelf)
        {
            OpenMenu();
            BuildUI.Instance.CanvasSort(true);
        }
        else
        {
            CloseMenu();
            BuildUI.Instance.CanvasSort(false);
        }
    }

    private void Rearrange()
    {
        float radianOfSeparation = (Mathf.PI * 2) / entries.Count;
        for (int i = 0; i < entries.Count; i++)
        {
            float x = Mathf.Sin(radianOfSeparation * i) * radius;
            float y = Mathf.Cos(radianOfSeparation * i) * radius;

            // entries[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
            RectTransform rect = entries[i].GetComponent<RectTransform>();
            
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad).SetDelay(.05f * i);
            rect.DOAnchorPos(new Vector3(x, y, 0), .3f).SetEase(Ease.OutQuad)
                .SetDelay(.05f * i);
        }
    }
}

[Serializable]
public class BuildList
{
    [SerializeField] public EntrieBuildIcon buildObject;
}

[Serializable]
public class EntrieBuildIcon
{
    public Texture icon;
    public GameObject prefab;

    public EntrieBuildIcon(Texture pIcon, GameObject pPrefab)
    {
        this.icon = pIcon;
        this.prefab = pPrefab;
    }
}
