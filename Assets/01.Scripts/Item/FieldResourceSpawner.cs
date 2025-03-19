using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldResourceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject resourcePrefab;
    private Transform spawnPivot;
    private GameObject instance;
    public int respawnCoolTime;

    private void Awake()
    {
        spawnPivot = this.transform;
    }

    private void Start()
    {
        Spawn();
    }

    public void Respawn()
    {
        StartCoroutine(respawnCo());
    }

    public IEnumerator respawnCo()
    {
        yield return new WaitForSeconds(respawnCoolTime);
        Spawn();
    }



    public void Spawn()
    {
        if(instance == null)
        {
            instance = Instantiate(resourcePrefab, spawnPivot);
        }
    }
}
