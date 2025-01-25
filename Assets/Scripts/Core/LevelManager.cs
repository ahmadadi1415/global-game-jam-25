using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<SceneConfigSO> levels = new();
    public static SceneConfigSO CurrentLevel { get; private set; }
    [SerializeField] private int currentLevelIndex = 0;

    private void Awake()
    {
        if (levels.Count == 0)
        {
            Debug.LogError("List of levels must not be empty!");
            return;
        }
    }
    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    private void OnEnable()
    {
        EventManager.Subscribe<OnLevelFinishedMessage>(OnLevelFinished);
        EventManager.Subscribe<OnNextLevelClickedMessage>(OnNextLevelClicked);
        EventManager.Subscribe<OnRestartLevelClickedMessage>(OnRestartLevelClicked);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OnLevelFinishedMessage>(OnLevelFinished);
        EventManager.Unsubscribe<OnNextLevelClickedMessage>(OnNextLevelClicked);
        EventManager.Unsubscribe<OnRestartLevelClickedMessage>(OnRestartLevelClicked);
    }

    private void OnRestartLevelClicked(OnRestartLevelClickedMessage message)
    {
        RestartLevel();
    }

    private void OnNextLevelClicked(OnNextLevelClickedMessage message)
    {
        LoadNextLevel();
    }


    private void OnLevelFinished(OnLevelFinishedMessage message)
    {
        if (!message.IsWin)
        {
            RestartLevel();
        }
    }

    private void LoadLevel(int index)
    {
        if (index >= levels.Count)
        {
            Debug.Log("All level is completed");
            EventManager.Publish<OnGameCompletedMessage>(new());
            return;
        }

        CurrentLevel = levels[index];
        // DO: Notify GameManager to update stats
        // DO: Notify GridManager to update the grids
        EventManager.Publish<OnLevelLoadedMessage>(new() { Level = CurrentLevel });
    }

    private void LoadNextLevel() => LoadLevel(currentLevelIndex += 1);
    private void RestartLevel() => LoadLevel(currentLevelIndex);
}