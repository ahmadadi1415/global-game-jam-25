using System;
using UnityEngine;

public enum GameState { PLAYING, LOSE, WIN };
public enum PowerUpType { BASIC, VERTICAL, HORIZONTAL, CROSS, SURROUND, TRIPLE };

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event Action OnLoseGame;
    public static event Action OnWinGame;

    public PowerUpType powerUp;
    public GameState gameState;

    public SceneConfigSO sceneConfig;
    private SceneConfigSO _currentSceneConfigSO;

    [SerializeField] private int _currentTurns = 0;
    [SerializeField] private int _maxTurns = 0;

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private PowerUpManager _powerUpManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (sceneConfig == null)
        {
            Debug.LogError("Please add Scene Config Scriptable Object To Game Manager");
        }
        else
        {
            if (sceneConfig != null)
            {
                _currentSceneConfigSO = sceneConfig;
                UpdateGameData(_currentSceneConfigSO);
            }
        }
    }

    private void UpdateGameData(SceneConfigSO level)
    {
        _currentTurns = level.MaxTurns;
        _maxTurns = level.MaxTurns;
    }

    private void OnEnable()
    {
        EventManager.Subscribe<OnBubblesCheckedMessage>(OnBubblesChecked);
        EventManager.Subscribe<OnLevelLoadedMessage>(OnLevelLoaded);
        // GridManager.OnUpdateGrid += Instance_OnUpdateGrid;
        GridManager.OnTurnEnd += Instance_OnTurnEnd;
        GridManager.OnUsePowerUp += Instance_OnUsePowerUp;
        // GridTileBase.OnGridClick += GridTileBase_OnGridClick;
        // OnWinGame += GameManager_OnWinGame;
        // OnLoseGame += GameManager_OnLoseGame;
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OnBubblesCheckedMessage>(OnBubblesChecked);
        EventManager.Unsubscribe<OnLevelLoadedMessage>(OnLevelLoaded);
        // GridManager.OnUpdateGrid -= Instance_OnUpdateGrid;
        GridManager.OnTurnEnd -= Instance_OnTurnEnd;
        GridManager.OnUsePowerUp -= Instance_OnUsePowerUp;
        // GridTileBase.OnGridClick -= GridTileBase_OnGridClick;
        // OnWinGame -= GameManager_OnWinGame;
        // OnLoseGame -= GameManager_OnLoseGame;
    }

    private void OnLevelLoaded(OnLevelLoadedMessage message)
    {
        gameState = GameState.PLAYING;
        UpdateGameData(message.Level);
    }

    private void OnBubblesChecked(OnBubblesCheckedMessage message)
    {
        if (message.IsAllPopped && _currentTurns >= 0)
        {
            gameState = GameState.WIN;
            Debug.Log("Game win");

            // DO: Continue to next levels
            EventManager.Publish<OnLevelFinishedMessage>(new() { IsWin = true });
        }
        else if (!message.IsAllPopped && _currentTurns == 0)
        {
            Debug.Log($"Is all popped: {message.IsAllPopped}, {_currentTurns}");
            gameState = GameState.LOSE;
            Debug.Log("Game lose");

            // DO: Restart the level
            EventManager.Publish<OnLevelFinishedMessage>(new() { IsWin = false });
        }
        else
        {
            gameState = GameState.PLAYING;
        }
    }

    private void Instance_OnUsePowerUp(GridManager.OnUsePowerUpEvent obj)
    {
        // set based on ui later
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
        if (_currentTurns == 1 && GridManager.Instance.bubbleTiles.Count > 0)
        {
            OnLoseGame?.Invoke();
        }

        if (_currentTurns >= 1 && GridManager.Instance.bubbleTiles.Count == 0)
        {
            OnWinGame?.Invoke();
        }
    }

    private void Instance_OnTurnEnd()
    {
        _currentTurns--;
        _powerUpManager.UsePowerUp(powerUp);
        SetPowerUp(PowerUpType.BASIC);
    }

    private void Instance_OnUpdateGrid()
    {
        Debug.Log("Grid updated");
    }

    public void SetPowerUp(PowerUpType powerUp)
    {
        if (powerUp == PowerUpType.BASIC)
        {
            this.powerUp = powerUp;
            return;
        }

        if (_powerUpManager.CanUsePowerUp(powerUp))
        {
            this.powerUp = powerUp;
        }
        else
        {
            Debug.Log($"Cant use this power up: {powerUp}");
            this.powerUp = PowerUpType.BASIC;
        }
    }

    public PowerUpType GetPowerUp()
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

    public SceneConfigSO GetSceneConfigSO()
    {
        return _currentSceneConfigSO;
    }
}

