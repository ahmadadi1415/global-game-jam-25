using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpStatsController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _horizontalLimitText, _verticalLimitText, _surroundLimitText, _crossLimitText, _tripleLimitText;
    Dictionary<PowerUpType, TextMeshProUGUI> _powerUpTexts;

    private void Awake()
    {
        _powerUpTexts = new Dictionary<PowerUpType, TextMeshProUGUI>
        {
            { PowerUpType.VERTICAL, _verticalLimitText},
            { PowerUpType.HORIZONTAL, _horizontalLimitText},
            { PowerUpType.SURROUND, _surroundLimitText},
            { PowerUpType.CROSS, _crossLimitText},
            { PowerUpType.TRIPLE, _tripleLimitText}
        };
    }

    private void OnEnable() {
        EventManager.Subscribe<OnPowerUpChangedMessage>(OnPowerUpUsed);
    }

    private void OnDisable() {
        EventManager.Unsubscribe<OnPowerUpChangedMessage>(OnPowerUpUsed);
    }

    private void OnPowerUpUsed(OnPowerUpChangedMessage message)
    {
        if (_powerUpTexts.ContainsKey(message.PowerUpType)) {
            _powerUpTexts[message.PowerUpType].text = message.CurrentLimit.ToString();
        }
    }
}