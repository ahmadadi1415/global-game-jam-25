using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Serializable]
    public enum GameState { Start, Lose, Win };
    [Serializable]
    public enum PowerUp { Basic, Vertical, Horizontal, Cross, Surround, Triple };

    public PowerUp _powerUp;
    public GameState _gameState;

    public int maxTurns = 20; // Maximum number of turns
    public Text turnsText; // UI Text to display remaining turns
    public Text statusText; // UI Text to display game status
    private int currentTurns;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        currentTurns = maxTurns;
        GridManager.Instance.OnUpdateGrid += Instance_OnUpdateGrid;
        UpdateUI();
    }

    private void Instance_OnUpdateGrid()
    {
        BubbleDestroyed();
    }

    public void SetPowerUp(PowerUp powerUp)
    {
        _powerUp = powerUp;
    }

    public PowerUp GetPowerUp()
    {
        return _powerUp;
    }

    public void SetGameState(GameState gameState)
    {
        _gameState = gameState;
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    public void BubbleDestroyed()
    {
        currentTurns--;
        CheckGameStatus();
        UpdateUI();
    }

    void CheckGameStatus()
    {
        if (GridManager.Instance.tiles.Count == 0 && currentTurns > 0)
        {
            //statusText.text = "You Win!";
            EndGame();
        }
        else if (currentTurns <= 0)
        {
            //statusText.text = "You Lose!";
            EndGame();
        }
    }

    void UpdateUI()
    {
        //turnsText.text = $"Turns: {currentTurns}";


        // add rest
    }

    void EndGame()
    {
        // Disable all bubbles
        foreach (var bubble in FindObjectsOfType<Bubble>())
        {
            bubble.enabled = false;
        }
    }
}

