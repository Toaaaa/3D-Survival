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
        itemData = DataManager.Instance.GetItemDataByID(itemKey);
        spawner = GetComponentInParent<FieldResourceSpawner>();
        if(spawner != null)
        {
            respawn = spawner.Respawn;
        }

    }

    public override void OnInteract()
    {
        var playerInventory = CharacterManager.Instance.Player.inventory;

        if (playerInventory != null)
        {
            for (int i = 0; i < amount; i++)
            {
                playerInventory.AddItem(itemData);
            }
            respawn?.Invoke();

            Destroy(gameObject);
        }
    }
}
