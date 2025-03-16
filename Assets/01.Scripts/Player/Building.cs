using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject previewPrefab; // 프리뷰 프리팹
    private GameObject currentPreview; // 현재 프리뷰 오브젝트
    private GameObject buildPrefabs;
    private Material[] previewMaterials; // 프리뷰 오브젝트의 재질
    private BoxCollider previewCollider;
    
    public bool runBuilding = false;        // 플레이어가 건축중일때
    private bool isBuilding = false;        // 건축가능 체크
    private bool isNeedCraft = false;       // 인벤토리 아이템 갯수 체크
    
    public float maxCheckDistance;
    private Vector3 previewRotation;
    public LayerMask layerMask;
    public LayerMask colliderMask;
    private Camera cam;
    public Camera Cam {set { cam = value; } }

    void Update()
    {

        if (currentPreview != null)
        {
            // 프리뷰 오브젝트가 마우스를 따라다니게 함
            FollowMouse();
            // 건축 가능 여부 확인 및 색상 변경
            UpdatePreviewColor();
            
            if (Input.GetKey(KeyCode.Q))
            {
                // Q키를 누르면 물체 회전
                previewRotation += Vector3.up;
            }
            
            if (Input.GetMouseButtonDown(0) && isBuilding)
            {
                // 좌클릭 및 초록색일 때 실제 오브젝트 생성
                PlaceObject();
            }
        }
        
    }

    public bool CheckForBuildingInInventory(BuildObject buildObject)
    {
        ItemSlot[] slots;
        slots = CharacterManager.Instance.Player.inventory.slots;
        int buildObjectNeedCount = 0;

        foreach (var needItemData in buildObject.needItems.needCraft)
        {
            foreach (var slot in slots)
            {
                if (needItemData.itemData == slot.ItemData)
                {
                    if (slot.quantity >= needItemData.needValue)
                    {
                        buildObjectNeedCount++;
                        if (buildObjectNeedCount == buildObject.needItems.needCraft.Length)
                        {
                            isNeedCraft = true;
                            return true;
                        }
                        break;
                    }
                }
            }
        }
        isNeedCraft = false;
        return false;
    }

    // 프리뷰 오브젝트 생성
    public void CreatePreviewObject(GameObject preview, GameObject buildObject)
    {
        cam = BuildManager.Instance.cameraController.CurCamera;
        previewPrefab = preview;
        buildPrefabs = buildObject;
        currentPreview = Instantiate(previewPrefab);
        
        previewCollider = currentPreview.GetComponent<BoxCollider>();

        // 모든 자식 오브젝트의 Renderer를 수집
        List<Material> materialList = new List<Material>();
        // 부모와 자식을 포함하여 모든 Renderer 탐색
        MeshRenderer[] renderers = currentPreview.GetComponentsInChildren<MeshRenderer>();
        foreach (Renderer renderer in renderers)
        {
            materialList.AddRange(renderer.materials);
        }
        previewMaterials = materialList.ToArray();


        foreach (Material material in previewMaterials)
        {
            if (material.HasProperty("_Color")) // 컬러 속성이 있는 경우에만
            {
                material.color = Color.green; // 초기 색상 초록색
            }
        }
        
        runBuilding = true;

    }

    // 프리뷰 오브젝트가 마우스를 따라다니게 함
    void FollowMouse()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit , maxCheckDistance, layerMask))
        {
            // 바닥의 법선 벡터 가져오기
            Vector3 normal = hit.normal;
            // 물체 회전 계산 (법선 방향으로 회전)
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
            
            currentPreview.transform.position = hit.point;
            currentPreview.transform.rotation = rotation;
            currentPreview.transform.Rotate(previewRotation);
        }
    }

    // 건축 가능 여부 확인 및 색상 변경
    void UpdatePreviewColor()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            if (hit.collider.gameObject.CompareTag("Ground") && !IsColliding())
            {
                foreach (Material material in previewMaterials)
                {
                    if (material.HasProperty("_Color")) // 컬러 속성이 있는 경우에만
                    {
                        material.color = Color.green; // 건축 가능
                    }
                }
                isBuilding = true;
            }
            else
            {
                foreach (Material material in previewMaterials)
                {
                    if (material.HasProperty("_Color"))  // 컬러 속성이 있는 경우에만
                    {
                        material.color = Color.red; // 건축 불가능
                    }
                }
                isBuilding = false;
            }
        }
    }

    // 다른 오브젝트와 충돌 여부 확인
    bool IsColliding()
    { 
        Vector3 worldSize = Vector3.Scale(previewCollider.size, previewPrefab.transform.lossyScale);
        
        Collider[] colliders = 
            Physics.OverlapBox(currentPreview.transform.position, 
                worldSize / 2, previewCollider.transform.rotation, colliderMask);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != currentPreview && !collider.gameObject.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    // 실제 오브젝트 생성
    void PlaceObject()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            // 바닥의 법선 벡터 가져오기
            Vector3 normal = hit.normal;
            // 물체 회전 계산 (법선 방향으로 회전)
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
            
            // previewPrefab은 isTrigger로 충돌하지 않게 해놔서 생성시 trigger off로 충돌가능하게
            GameObject newObject = Instantiate(buildPrefabs, currentPreview.transform.position, rotation);
            newObject.transform.Rotate(previewRotation);
            
            BuildObject buildObject = buildPrefabs.gameObject.GetComponent<BuildObject>();
            ResumeInventoryItems(buildObject);
            
            previewMaterials = null;
            previewPrefab = null;
            buildPrefabs = null;
            previewRotation = Vector3.zero;
            Destroy(currentPreview);
        }
        runBuilding = false;
    }

    private void ResumeInventoryItems(BuildObject buildObject)
    {
        ItemSlot[] slots;
        slots = CharacterManager.Instance.Player.inventory.slots;
        int buildObjectNeedCount = 0;

        foreach (var needItemData in buildObject.needItems.needCraft)
        {
            foreach (var slot in slots)
            {
                if (needItemData.itemData == slot.ItemData)
                {
                    if (slot.quantity >= needItemData.needValue)
                    {
                        slot.quantity -= needItemData.needValue;
                        buildObjectNeedCount++;
                        if (buildObjectNeedCount == buildObject.needItems.needCraft.Length)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

    public void ClearPreview()
    {
        runBuilding = false;
        previewMaterials = null;
        previewPrefab = null;
        buildPrefabs = null;
        Destroy(currentPreview);
        currentPreview = null;
    }
}