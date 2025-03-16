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
    public PlayerEquipment equipment;
    public PlayerAttack playerAttack;
    public Building building;
    public Interanction interact;

    [HideInInspector] public bool isDead = false;

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
        interact = GetComponent<Interanction>();
        equipment = GetComponent<PlayerEquipment>();
    }

    private void Update()
    {
        if (isDead)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
