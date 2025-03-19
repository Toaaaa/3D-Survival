using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance;
    public static MonsterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("MonsterManager").AddComponent<MonsterManager>();
            }
            return instance;
        }
    }

    public MonsterDropManager monsterDropManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        monsterDropManager = GetComponent<MonsterDropManager>();
    }
}
