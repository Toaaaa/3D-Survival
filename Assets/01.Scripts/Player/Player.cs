using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerHandler handler;
    public PlayerCondition condition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        handler = GetComponent<PlayerHandler>();
        condition = GetComponent<PlayerCondition>();        
    }
}
