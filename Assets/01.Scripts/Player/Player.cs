using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerHandler handler;
    public PlayerCondition condition;
    public InputController input;

    private void Awake()
    {        
        CharacterManager.Instance.Player = this;
        if (input == null)
        {
            input = gameObject.AddComponent<InputController>();
        }    
        handler = GetComponent<PlayerHandler>();   
        condition = GetComponent<PlayerCondition>();      
    }
}
