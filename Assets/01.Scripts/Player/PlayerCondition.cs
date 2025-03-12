using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [Header("Conditions")]
    [SerializeField] Condition[] conditionsArray = new Condition[4];
    public Condition[] ConditionsArray { get => conditionsArray; }
    private Dictionary<ConditionType, Condition> conditions = new Dictionary<ConditionType, Condition>();
    public Dictionary<ConditionType, Condition> Conditions { get => conditions; }

    private void Awake()
    {
        foreach (var condition in conditionsArray)
        {
            conditions[condition.Type] = condition;
        }

        foreach (var condition in conditions.Values)
        {
            condition.CurValue = condition.MaxValue;
        }
    }

    private void Update()
    {
        PassiveCondition();
    }

    private void PassiveCondition()
    {
        foreach (var condition in conditions.Values)
        {
            if (condition.Type == ConditionType.Water || condition.Type == ConditionType.Hunger)
            {
                condition.PassiveSubtract();
            }

            if (condition.Type == ConditionType.Hunger && condition.curValue <= 0)
            {
                if (conditions.TryGetValue(ConditionType.Health, out Condition health))
                {
                    health.PassiveSubtract();
                }
            }
        }
    }
}
