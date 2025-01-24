using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action OnLoseGame;
    public event Action OnWinGame;

    [Serializable]
    public enum GameState { Start, Lose, Win };
    [Serializable]
    public enum PowerUp { Basic, Vertical, Horizontal, Cross, Surround, Triple };

    public PowerUp powerUp;
    public GameState gameState;

    public SceneConfigSO sceneConfig;

    private int _currentTurns = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _currentTurns = sceneConfig.MaxTurns;
        GridManager.Instance.OnUpdateGrid += Instance_OnUpdateGrid;
        GridManager.Instance.OnTurnEnd += Instance_OnTurnEnd;
        GridTileBase.OnGridClick += GridTileBase_OnGridClick;
        OnWinGame += GameManager_OnWinGame;
        OnLoseGame += GameManager_OnLoseGame;
    }

    private void GameManager_OnLoseGame()
    {
        Debug.Log("Lose");
    }

    private void GameManager_OnWinGame()
    {
        Debug.Log("Winner");
    }

    private void GridTileBase_OnGridClick(GridTileBase.OnGridClickEvent obj)
    {
        if (_currentTurns == 1 && GridManager.Instance.tiles.Count > 0 )
        {
            OnLoseGame?.Invoke();
        }

        if(_currentTurns >= 1 && GridManager.Instance.tiles.Count == 0)
        {
            OnWinGame?.Invoke();
        }
    }

    private void Instance_OnTurnEnd()
    {
        _currentTurns--;
        Debug.Log(_currentTurns);
    }

    private void Instance_OnUpdateGrid()
    {

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
}

