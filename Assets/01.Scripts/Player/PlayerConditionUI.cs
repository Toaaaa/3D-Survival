using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConditionUI : MonoBehaviour
{
    [SerializeField] ConditionType type;
    [SerializeField] Image uiBar;

    Condition condition;

    private void Start()
    {
        SetType();
    }

    private void Update()
    {
        uiBar.fillAmount = condition.GetPercentage();
    }

    private void SetType()
    {
        var _condition = CharacterManager.Instance.Player.condition.Conditions;

        if (_condition.TryGetValue(type, out condition))
        {
            return;
        }
        
    }
}
