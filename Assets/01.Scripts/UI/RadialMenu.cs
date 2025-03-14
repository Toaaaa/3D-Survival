using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class RadialMenu : MonoBehaviour
{
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private float radius = 300f;
    [SerializeField] List<Texture> icons;
    [SerializeField] List<GameObject> buildPrefabs;

    [SerializeField] public GameObject nameObject;
    [SerializeField] public GameObject descriptionObject;
    
    private List<RadialMenuEntry> entries;

    private void Awake()
    {
        BuildUI.Instance.radialMenu = this;
    }

    private void Start()
    {
        entries = new List<RadialMenuEntry>();
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
        Cursor.lockState = CursorLockMode.None;
        for (int i = 0; i < 5; i++)
        {
            AddEntry(icons[i], buildPrefabs[i]);
        }
        Rearrange();
    }

    public void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        for (int i = 0; i < 5; i++)
        {
            RectTransform rect = entries[i].GetComponent<RectTransform>();
            GameObject entry = entries[i].gameObject;

            rect.DOAnchorPos(Vector3.zero, .3f).SetEase(Ease.OutQuad).onComplete
                = delegate()
                {
                    Destroy(entry);
                };
        }
        
        nameObject.SetActive(false);
        descriptionObject.SetActive(false);
        
        entries.Clear();
    }

    public void Toggle()
    {
        if (entries.Count == 0)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
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
