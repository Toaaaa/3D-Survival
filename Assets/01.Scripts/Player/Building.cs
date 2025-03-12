using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject previewPrefab; // 프리뷰 프리팹
    private GameObject currentPreview; // 현재 프리뷰 오브젝트
    private Material[] previewMaterials; // 프리뷰 오브젝트의 재질
    
    private bool isBuilding = false;
    
    public float maxCheckDistance;
    public LayerMask layerMask;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && currentPreview == null)
        {
            // B키를 눌렀을 때 프리뷰 오브젝트 생성
            CreatePreviewObject();
        }

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
    void CreatePreviewObject()
    {
        currentPreview = Instantiate(previewPrefab);
        previewMaterials = currentPreview.GetComponent<MeshRenderer>().materials;
        
        foreach (Material material in previewMaterials)
        {
            if (material.HasProperty("_Color"))  // 컬러 속성이 있는 경우에만
            {
                material.color = Color.green; // 초기 색상 초록색
            }
        }
        
    }

    // 프리뷰 오브젝트가 마우스를 따라다니게 함
    void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
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
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            if (hit.collider.gameObject.CompareTag("Terrain") && !IsColliding())
            {
                foreach (Material material in previewMaterials)
                {
                    if (material.HasProperty("_Color"))  // 컬러 속성이 있는 경우에만
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
        Collider[] colliders = Physics.OverlapBox(currentPreview.transform.position, currentPreview.transform.localScale / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != currentPreview && !collider.gameObject.CompareTag("Terrain"))
            {
                return true;
            }
        }
        return false;
    }

    // 실제 오브젝트 생성
    void PlaceObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
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
            Destroy(currentPreview);
        }
    }
}