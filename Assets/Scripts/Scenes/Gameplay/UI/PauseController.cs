using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
        });

        restartButton.onClick.AddListener(() => {
            EventManager.Publish<OnLevelFinishedMessage>(new() { IsWin = false });
        });

        quitButton.onClick.AddListener(() => { 
        // ini kemana? home?
        });
    }
}
