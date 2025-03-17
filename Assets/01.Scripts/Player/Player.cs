using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerHandler handler;
    [HideInInspector] public PlayerCondition condition;
    [HideInInspector] public InputController input;
    [HideInInspector] public PlayerInventory inventory;
    [HideInInspector] public PlayerEquipment equipment;
    [HideInInspector] public PlayerAttack playerAttack;
    [HideInInspector] public Building building;
    [HideInInspector] public Interanction interact;

    [SerializeField] GameObject gamoverUI;
    [SerializeField] Button restartBtn;

    public bool isDead = false;

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

        restartBtn.onClick.AddListener(OnRestart);
    }

    private void Update()
    {
        if (isDead)
        {
            Cursor.lockState = CursorLockMode.None;
            gamoverUI.SetActive(true);
        }
    }

    private void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     
    }
}
