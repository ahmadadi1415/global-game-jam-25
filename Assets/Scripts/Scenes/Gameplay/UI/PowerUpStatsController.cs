using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private void OnEnable()
    {
        EventManager.Subscribe<OnPowerUpChangedMessage>(OnPowerUpChanged);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OnPowerUpChangedMessage>(OnPowerUpChanged);
    }

    private void OnPowerUpChanged(OnPowerUpChangedMessage message)
    {
        if (_powerUpTexts.ContainsKey(message.PowerUpType))
        {
            int currentLimit = message.CurrentLimit;
            _powerUpTexts[message.PowerUpType].text = currentLimit.ToString();

            bool interactable = currentLimit > 0;
            Button button = _powerUpTexts[message.PowerUpType].GetComponentInParent<Button>();
            button.interactable = interactable;

            if (!interactable)
            {
                ColorBlock colors = button.colors;
                colors.disabledColor = new Color(colors.disabledColor.r, colors.disabledColor.g, colors.disabledColor.b, 0f); // Set transparency
                button.colors = colors;
            }
        }
    }
}