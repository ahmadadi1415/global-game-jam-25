using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText; // Reference to the UI text component that displays the score

    /// <summary>
    /// Called when the object is enabled. Subscribes to the OnScoreChangedMessage event to update the score UI.
    /// </summary>
    private void OnEnable()
    {
        // Subscribe to the OnScoreChangedMessage event
        EventManager.Subscribe<OnScoreChangedMessage>(OnScoreChanged);
    }

    /// <summary>
    /// Called when the object is disabled. Unsubscribes from the OnScoreChangedMessage event to avoid memory leaks.
    /// </summary>
    private void OnDisable()
    {
        // Unsubscribe from the OnScoreChangedMessage event
        EventManager.Unsubscribe<OnScoreChangedMessage>(OnScoreChanged);
    }

    /// <summary>
    /// Callback method that updates the score displayed in the UI when the score changes.
    /// </summary>
    /// <param name="message">The message containing the updated score.</param>
    private void OnScoreChanged(OnScoreChangedMessage message)
    {
        // Extract the updated score from the message
        int score = message.Score;

        // Update the UI text component to display the new score
        _scoreText.text = $"Score: {score}";
    }

}