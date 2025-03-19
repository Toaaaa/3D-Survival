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

    //플레이어 컨디션 확인 후 세팅
    private void SetType()
    {
        var _condition = CharacterManager.Instance.Player.condition.Conditions;

        if (_condition.TryGetValue(type, out condition))
        {
            return;
        }
    }
}
