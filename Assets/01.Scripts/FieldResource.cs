using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldResource : ItemObject
{
    public Action respawn;
    private FieldResourceSpawner spawner;

    private void Start()
    {
        spawner = GetComponentInParent<FieldResourceSpawner>();
        respawn = spawner.Respawn;
    }

    public override void OnInteract()
    {
        //아이템데이터에 넣어줌
        //CharacterManager.Instance.Player.ItemData = itemData;
        //액션
        //CharacterManager.Instance.Player.AddItem.Invoke();
        respawn?.Invoke();

        Destroy(gameObject);
    }
}
