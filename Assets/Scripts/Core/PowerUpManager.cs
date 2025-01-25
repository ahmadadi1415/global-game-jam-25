using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private Dictionary<PowerUpType, (int current, int max)> _powerUpCounts;

    private void Awake()
    {
        // Initialize the power-up counts and max limits
        InitPowerUpData();
    }

    private void InitPowerUpData()
    {
        _powerUpCounts = new Dictionary<PowerUpType, (int, int)>
        {
            { PowerUpType.VERTICAL, (0, 0) },
            { PowerUpType.HORIZONTAL, (0, 0) },
            { PowerUpType.SURROUND, (0, 0) },
            { PowerUpType.CROSS, (0, 0) },
            { PowerUpType.TRIPLE, (0, 0) }
        };
    }

    private void OnEnable()
    {
        EventManager.Subscribe<OnLevelLoadedMessage>(OnLevelLoaded);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OnLevelLoadedMessage>(OnLevelLoaded);
    }

    private void OnLevelLoaded(OnLevelLoadedMessage message)
    {
        SceneConfigSO level = message.Level;
        UpdatePowerUpCounts(level);
    }

    private void UpdatePowerUpCounts(SceneConfigSO level)
    {
        if (_powerUpCounts.ContainsKey(PowerUpType.VERTICAL))
            _powerUpCounts[PowerUpType.VERTICAL] = (level.VerticalLimit, level.VerticalLimit);

        if (_powerUpCounts.ContainsKey(PowerUpType.HORIZONTAL))
            _powerUpCounts[PowerUpType.HORIZONTAL] = (level.HorizontalLimit, level.HorizontalLimit);

        if (_powerUpCounts.ContainsKey(PowerUpType.SURROUND))
            _powerUpCounts[PowerUpType.SURROUND] = (level.SurroundLimit, level.SurroundLimit);

        if (_powerUpCounts.ContainsKey(PowerUpType.CROSS))
            _powerUpCounts[PowerUpType.CROSS] = (level.CrossLimit, level.CrossLimit);

        if (_powerUpCounts.ContainsKey(PowerUpType.TRIPLE))
            _powerUpCounts[PowerUpType.TRIPLE] = (level.TripleLimit, level.TripleLimit);
    }

    public bool UsePowerUp(PowerUpType powerUpType)
    {
        if (_powerUpCounts.ContainsKey(powerUpType))
        {
            (int current, int max) = _powerUpCounts[powerUpType];

            if (current <= 0) return false;

            _powerUpCounts[powerUpType] = (current - 1, max);
            return true;
        }

        return false;
    }

    public bool CanUsePowerUp(PowerUpType powerUpType)
    {
        if (_powerUpCounts.ContainsKey(powerUpType))
        {
            (int current, int max) = _powerUpCounts[powerUpType];

            return current > 0;
        }

        return false;
    }

    public int GetCurrentCount(PowerUpType powerUpType) => _powerUpCounts.ContainsKey(powerUpType) ? _powerUpCounts[powerUpType].current : 0;
    public int GetMaxLimit(PowerUpType powerUpType) => _powerUpCounts.ContainsKey(powerUpType) ? _powerUpCounts[powerUpType].max : 0;
}