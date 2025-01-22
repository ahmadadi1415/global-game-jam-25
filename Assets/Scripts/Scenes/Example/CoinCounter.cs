using System.Collections;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private float incrementDelaySeconds = 1;
    public int Score { get; private set; } = 0;

    // Start is called before the first frame update
    private void Start()
    {
        // Starts a coroutine to increment the coin count periodically with a specified delay
        StartCoroutine(IncrementCoin(incrementDelaySeconds));
    }

    /// <summary>
    /// A coroutine that increments the score periodically based on the specified delay.
    /// </summary>
    /// <param name="delaySeconds">The delay in seconds between each score increment.</param>
    /// <returns>Yields a WaitForSeconds for the given delay before resuming execution.</returns>
    private IEnumerator IncrementCoin(float delaySeconds)
    {
        while (true) // Infinite loop to continually update the score
        {
            // Wait for the specified delay before executing the next iteration
            yield return new WaitForSeconds(delaySeconds);

            // Increment the score by 1
            Score++;

            // Publish an event to notify other parts of the system that the score has changed
            EventManager.Publish<OnScoreChangedMessage>(new() { Score = Score });
        }
    }

}