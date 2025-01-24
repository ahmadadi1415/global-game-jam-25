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

    public PowerUp powerUp;
    public GameState gameState;

    //public Text turnsText; // UI Text to display remaining turns
    //public Text statusText; // UI Text to display game status

    public SceneConfigSO sceneConfig;

    private int _currentTurns;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _currentTurns = sceneConfig.MaxTurns;
        GridManager.Instance.OnUpdateGrid += Instance_OnUpdateGrid;
        UpdateUI();
    }

    private void Instance_OnUpdateGrid()
    {
        BubbleDestroyed();
    }

    public void SetPowerUp(PowerUp powerUp)
    {
        this.powerUp = powerUp;
    }

    public PowerUp GetPowerUp()
    {
        return powerUp;
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public int GetTurnRemain()
    {
        return _currentTurns;
    }
    public void BubbleDestroyed()
    {
        _currentTurns--;
        CheckGameStatus();
        UpdateUI();
    }

    void CheckGameStatus()
    {
        if (GridManager.Instance.tiles.Count == 0 && _currentTurns > 0)
        {
            //statusText.text = "You Win!";
            EndGame();
        }
        else if (_currentTurns <= 0)
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
        
    }
}

