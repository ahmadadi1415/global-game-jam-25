using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject _pausePanel;
    void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            EventManager.Publish<OnPlayStateChangedMessage>(new() { IsPlaying = true });
        });

        restartButton.onClick.AddListener(() =>
        {
            EventManager.Publish<OnPlayStateChangedMessage>(new() { IsPlaying = true });
            EventManager.Publish<OnRestartLevelClickedMessage>(new());
        });

        quitButton.onClick.AddListener(() =>
        {
            // ini kemana? home?
            SceneManager.LoadScene("MainMenu");
        });
    }

    private void OnEnable()
    {
        EventManager.Subscribe<OnPlayStateChangedMessage>(OnPlayStateChanged);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OnPlayStateChangedMessage>(OnPlayStateChanged);
    }

    private void OnPlayStateChanged(OnPlayStateChangedMessage message)
    {
        if (!message.IsPlaying)
        {
            UIAnimation.ZoomIn(gameObject, 0.5f).setOnStart(() => _pausePanel.SetActive(true));
        }
        else
        {
            UIAnimation.ZoomOut(gameObject, 0.5f).setOnComplete(() => _pausePanel.SetActive(false));
        }
    }
}
