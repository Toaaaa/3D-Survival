using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    InputController controller;
    PlayerCondition condition;

    private void Awake()
    {
        controller = GetComponent<InputController>();
        condition = GetComponent<PlayerCondition>();
    }


}
