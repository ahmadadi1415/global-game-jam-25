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

    private void OnEnable()
    {
        EventManager.Subscribe<OnLevelFinishedMessage>(OnLevelFinished);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OnLevelFinishedMessage>(OnLevelFinished);
    }

    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    private void OnLevelFinished(OnLevelFinishedMessage message)
    {
        if (message.IsWin)
        {
            LoadNextLevel();
        }
        else
        {
            RestartLevel();
        }
    }

    private void LoadLevel(int index)
    {
        if (index >= levels.Count)
        {
            Debug.Log("All level is completed");
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