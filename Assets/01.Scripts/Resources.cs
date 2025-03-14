using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public ItemData itemToGive;
    //public int quantityPerHit = 1;
    public int totalquantity;
    public int originCapacy;
    public int capacy;
    public float respawnCoolTime;

    private Collider resourceCollider;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        resourceCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        //var Materials  = meshRenderer.materials;
        //for (int i = 0; i < Materials.Length; i++)
        //{
        //    Materials[i] = Instantiate(Materials[i]);
        //}
    }

    private void Start()
    {
        capacy = originCapacy;
    }

    public virtual void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        //for (int i = 0; i < quantityPerHit; i++)
        //{
        //    if (capacy <= 0) break;
        //    capacy--;
        //    Instantiate(itemToGive.drobPrefab, hitPoint + Vector3.up+hitNormal, Quaternion.LookRotation(hitNormal, Vector3.up));
        //}
        capacy -= 1;//장비에 달린 수치만큼 capacy를 빼도록 바꿔야됨
        if (capacy <= 0)
        {
            
            ItemObject itemObject = Instantiate(itemToGive.drobPrefab, hitPoint + Vector3.up + hitNormal, Quaternion.LookRotation(hitNormal, Vector3.up)).GetComponent<ItemObject>();
            itemObject.amount = totalquantity;
            StartCoroutine(RespawnCo());
        }
    }

    public void Disappear()
    {
        resourceCollider.enabled = false;
        meshRenderer.enabled = false;
    }

    public void Respawn()
    {
        resourceCollider.enabled = true;
        meshRenderer.enabled = true;
        capacy = originCapacy;
    }
    private IEnumerator RespawnCo()
    {
        Disappear();
        yield return new WaitForSeconds(respawnCoolTime);
        Respawn();
    }

    //private IEnumerator ChangeColorCo(float targetAlpha)
    //{

    //    float originAlpha = meshRenderer.material.color.a;
    //    while (meshRenderer.material.color.a != targetAlpha)
    //    {
    //        Debug.Log("색바꾸기 시작");
    //        for (int j = 1; j < 11; j++)
    //        {
    //            for (int i = 0; i < meshRenderer.materials.Length; i++)
    //            {
    //                Color color = meshRenderer.materials[i].color; // 기존 색상을 가져옴
    //                color.a = Mathf.Lerp(originAlpha, targetAlpha, j / 10f); // 알파 값만 변경
    //                meshRenderer.materials[i].color = color; // 변경된 색상을 다시 할당
    //            }
    //            yield return new WaitForSeconds(0.1f);
    //        }

    //    }
    //}
}
