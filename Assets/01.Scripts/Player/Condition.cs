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

    public void Add(float value)
    {
        if (curValue == maxValue) return;
        curValue = Mathf.Min(value + curValue, maxValue);
    }

    public void Subtract(float value)
    {
        if (curValue == 0) return; 
        curValue = Mathf.Max(curValue - value, 0);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void PassiveSubtract()
    {
        Subtract(passiveValue * Time.deltaTime);
    }
}
