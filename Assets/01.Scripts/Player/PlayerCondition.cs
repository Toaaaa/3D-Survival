using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [Header("Conditions")]
    [SerializeField] Condition[] conditionsArray = new Condition[4];
    public Condition[] ConditionsArray { get => conditionsArray; }
    Dictionary<ConditionType, Condition> conditions = new Dictionary<ConditionType, Condition>();
    public Dictionary<ConditionType, Condition> Conditions { get => conditions; }
    
    [HideInInspector]public bool isFreezing = false;
    PlayerAnimator animator;
    CameraController cameraController;

    private void Awake()
    {
        //인스펙터배열->딕셔너리 변환
        foreach (var condition in conditionsArray)
        {
            conditions[condition.Type] = condition;
        }
       //초기값 설정
        foreach (var condition in conditions.Values)
        {
            condition.CurValue = condition.MaxValue;
        }
        animator = GetComponent<PlayerAnimator>();
        cameraController = GetComponentInChildren<CameraController>();
    }

    private void Update()
    {
        PassiveCondition();

        if (conditions.TryGetValue(ConditionType.Health, out Condition health))
        {
            if(health.curValue <= 0)
            {
                if (CharacterManager.Instance.Player.isDead) return;

                IsDead();
            }
        }
    }

    private void PassiveCondition()
    {
        if (conditions.TryGetValue(ConditionType.Water, out Condition water))
        {
            water.PassiveChanging();
            isFreezing = water.curValue <= 0 ? true : false;
        }

        if (conditions.TryGetValue(ConditionType.Hunger, out Condition hunger))
        {
            hunger.PassiveChanging();

            // Hunger가 0 이하일 경우 Health 감소
            if (hunger.CurValue <= 0)
            {
                if (conditions.TryGetValue(ConditionType.Health, out Condition health))
                {
                    health.PassiveChanging();
                }
            }
        }

        if (conditions.TryGetValue(ConditionType.Stamina, out Condition stamina))
        {
            stamina.PassiveChanging();
        }
    }

    public bool UseStamina(float value)
    {
        if(conditions.TryGetValue(ConditionType.Stamina, out Condition stamina))
        {
            if (stamina.curValue + value <= 0)
            {
                return false;
            }
            stamina.ChangCondition(value);
        }
        return true;
    }

    private void IsDead()
    {
        animator.PlayerDie();
        CharacterManager.Instance.Player.isDead = true;        
    }
}
