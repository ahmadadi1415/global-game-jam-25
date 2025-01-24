using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int maxTurns = 20; // Maximum number of turns
    public Text turnsText; // UI Text to display remaining turns
    public Text statusText; // UI Text to display game status
    private int currentTurns;
    private int totalBubbles;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        currentTurns = maxTurns;
        totalBubbles = FindObjectsOfType<Bubble>().Length; // Count all bubbles in the scene
        UpdateUI();
    }

    public void BubbleDestroyed()
    {
        totalBubbles--;
        currentTurns--;
        CheckGameStatus();
        UpdateUI();
    }

    void CheckGameStatus()
    {
        if (totalBubbles == 0)
        {
            statusText.text = "You Win!";
            EndGame();
        }
        else if (currentTurns <= 0)
        {
            statusText.text = "You Lose!";
            EndGame();
        }
    }

    void UpdateUI()
    {
        turnsText.text = $"Turns: {currentTurns}";
    }

    void EndGame()
    {
        // Disable all bubbles
        foreach (var bubble in FindObjectsOfType<Bubble>())
        {
            bubble.enabled = false;
        }
    }
}

