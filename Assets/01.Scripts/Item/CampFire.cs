using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    [SerializeField] private float liveTime;

    private void Awake()
    {
        StartCoroutine(liveFire());
    }


    IEnumerator liveFire()
    {
        yield return new WaitForSeconds(liveTime);
        Destroy(gameObject);
    }

}
