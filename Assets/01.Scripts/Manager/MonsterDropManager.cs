using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class MonsterDropManager : MonoBehaviour
{
    public List<MonsterDropTable> monsterDrops = new(); // 드랍 정보 리스트
    private Dictionary<string, List<MonsterDrop>> dropTable = new();

    private void Awake()
    {
        foreach (var monsterDrop in monsterDrops)
        {
            dropTable.Add(monsterDrop.monsterName, monsterDrop.dropInfo);
        }
    }

    public void DropItem(string monsterName, Vector3 dropPos)
    {
        if (!dropTable.ContainsKey(monsterName)) return;

        foreach (var dropItem in dropTable[monsterName])
        {
            if (Random.value <= dropItem.dropRate) // 확률 체크
            {
                Instantiate(dropItem.item, dropPos + new Vector3(0,0.3f,0), Quaternion.identity); // 아이템 생성
            }
        }
    }
}

[System.Serializable]
public class MonsterDropTable
{
    public string monsterName;
    public List<MonsterDrop> dropInfo; // itemdata와 dropRate를 가지고 있는 리스트
}
[System.Serializable]
public class MonsterDrop
{
    [SerializeField] private GameObject Item;
    [SerializeField] private float DropRate;
    // 읽기 전용 프로퍼티
    public GameObject item => Item;
    public float dropRate => DropRate;
}
