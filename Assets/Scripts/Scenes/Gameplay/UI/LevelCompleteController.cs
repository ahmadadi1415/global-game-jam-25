using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteController : MonoBehaviour
{
    [SerializeField] private Button quitBtn, nextLevelBtn;
    [SerializeField] private GameObject _levelCompletedUI;

    void Start()
    {
        quitBtn.onClick.AddListener(OnQuitClick);
        nextLevelBtn.onClick.AddListener(OnNextLevelClick);
    }

    private void OnEnable() {
        EventManager.Subscribe<OnLevelFinishedMessage>(OnLevelFinished);
    }
    private void OnDisable() {
        EventManager.Unsubscribe<OnLevelFinishedMessage>(OnLevelFinished);
    }

    private void OnLevelFinished(OnLevelFinishedMessage message)
    {
        _levelCompletedUI.SetActive(message.IsWin);
        UIAnimation.ZoomIn(gameObject, 0.5f, true);
    }

    private void OnQuitClick()
    {
        // scene manager for main menu
        SceneManager.LoadScene("MainMenu");
    }
    private void OnNextLevelClick()
    {
        // EventManager.Publish<OnLevelFinishedMessage>(new() { IsWin = true });
        _levelCompletedUI.SetActive(false);
        UIAnimation.ZoomOut(gameObject, 0.5f, true);
        EventManager.Publish<OnNextLevelClickedMessage>(new());
    }

}
