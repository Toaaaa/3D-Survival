using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerHandler handler;
    public PlayerCondition condition;
    public InputController input;
    public PlayerInventory inventory;
    public PlayerAttack playerAttack;
    public Building building;


    private void Awake()
    {        
        CharacterManager.Instance.Player = this;
        if (input == null)
        {
            input = gameObject.AddComponent<InputController>();
        }    
        handler = GetComponent<PlayerHandler>();   
        condition = GetComponent<PlayerCondition>();
        inventory = GetComponent<PlayerInventory>();
        playerAttack = GetComponent<PlayerAttack>();
        building = GetComponent<Building>();
    }
}
