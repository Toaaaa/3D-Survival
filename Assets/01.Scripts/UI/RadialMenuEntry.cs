using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class RadialMenuEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private RawImage icon;
    [SerializeField] private GameObject prefab;
    private BuildObject buildObject;

    private RectTransform rect;

    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }

    private void Start()
    {
        rect = icon.GetComponent<RectTransform>();
        buildObject = prefab.gameObject.GetComponent<BuildObject>();
        name = buildObject.name;
        description = buildObject.GetNameXValues();
    }

    public void SetIcon(Texture pIcon)
    {
        icon.texture = pIcon;
    }

    public void SetPrefab(GameObject pPrefab)
    {
        prefab = pPrefab;
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        // 건물소환
        // if(CharacterManager.Instance.Player.building.CheckForBuildingInInventory(buildObject))
            CharacterManager.Instance.Player.building.CreatePreviewObject(buildObject.previewPrefab, prefab);

        BuildUI.Instance.radialMenu.Toggle();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BuildUI.Instance.radialMenu.nameObject.gameObject.SetActive(true);
        BuildUI.Instance.radialMenu.descriptionObject.gameObject.SetActive(true);
        
        BuildUI.Instance.radialMenu.nameObject.text = name;
        BuildUI.Instance.radialMenu.descriptionObject.text = description;
        
        rect.DOComplete();
        rect.DOScale(Vector3.one * 1.5f, .3f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BuildUI.Instance.radialMenu.nameObject.gameObject.SetActive(false);
        BuildUI.Instance.radialMenu.descriptionObject.gameObject.SetActive(false);
        
        rect.DOComplete();
        rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
    }
}
