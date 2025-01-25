using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteController : MonoBehaviour
{
    [SerializeField] private Button quitBtn, nextLevelBtn;

    void Start()
    {
        quitBtn.onClick.AddListener(OnQuitClick);
        nextLevelBtn.onClick.AddListener(OnNextLevelClick);
    }

    private void OnQuitClick()
    {
        // scene manager for main menu
    }
    private void OnNextLevelClick()
    {
        EventManager.Publish<OnLevelFinishedMessage>(new() { IsWin = true });
    }
}
