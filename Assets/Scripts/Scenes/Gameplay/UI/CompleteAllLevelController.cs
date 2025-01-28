using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompleteAllLevelController : MonoBehaviour
{
    [SerializeField] private Button quitBtn;
    [SerializeField] private GameObject _gameCompletedUI;
    void Start()
    {
        quitBtn.onClick.AddListener(OnQuitClick);
    }

    private void OnEnable()
    {
        EventManager.Subscribe<OnGameCompletedMessage>(OnGameCompleted);
    }

    private void OnDisable() {
        EventManager.Unsubscribe<OnGameCompletedMessage>(OnGameCompleted);
    }

    private void OnGameCompleted(OnGameCompletedMessage message)
    {
        _gameCompletedUI.SetActive(true);
    }

    private void OnQuitClick()
    {
        // scene manager to main menu
        SceneManager.LoadScene("MainMenu");
    }
}
