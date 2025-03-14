using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject previewPrefab; // 프리뷰 프리팹
    private GameObject currentPreview; // 현재 프리뷰 오브젝트
    private Material[] previewMaterials; // 프리뷰 오브젝트의 재질
    private BoxCollider previewCollider;
    
    public bool runBuilding = false;        // 플레이어가 건축중일때
    private bool isBuilding = false;        // 건축가능 체크
    
    public float maxCheckDistance;
    public LayerMask layerMask;
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
            
            if (Input.GetMouseButtonDown(0) && isBuilding)
            {
                // 좌클릭 및 초록색일 때 실제 오브젝트 생성
                PlaceObject();
            }
        }
        
    }

    // 프리뷰 오브젝트 생성
    public void CreatePreviewObject(GameObject preview)
    {
        cam = BuildManager.Instance.cameraController.CurCamera;
        previewPrefab = preview;
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
        
        Collider[] colliders = Physics.OverlapBox(currentPreview.transform.position, worldSize / 2, previewCollider.transform.rotation);
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
            GameObject newObject = Instantiate(previewPrefab, currentPreview.transform.position, rotation);
            Collider newCollider = newObject.GetComponent<Collider>();
            newCollider.isTrigger = false;
            previewPrefab = null;
            Destroy(currentPreview);
        }
        runBuilding = false;
    }

    public void ClearPreview()
    {
        runBuilding = false;
        previewPrefab = null;
        Destroy(currentPreview);
        currentPreview = null;
    }
}