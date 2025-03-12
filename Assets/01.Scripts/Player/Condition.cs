using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    Health,
    Hunger,
    Water,
    Stamina
}

[Serializable]
public class Condition
{
    [SerializeField] ConditionType type;
    public ConditionType Type { get => type; }

    public float curValue;
    public float CurValue { get => curValue; set => curValue = value; }
    
    [SerializeField] float maxValue;
    [SerializeField] float passiveValue;

    public float MaxValue { get => maxValue; }

    public void ChangCondition(float value)
    {
        if (curValue > maxValue || curValue == 0) return;
        curValue = Mathf.Clamp(curValue + value, 0 , maxValue);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void PassiveChanging()
    {
        ChangCondition(passiveValue * Time.deltaTime);
    }
}
