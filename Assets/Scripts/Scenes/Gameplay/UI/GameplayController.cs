using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public bool IsPlaying = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPlaying = !IsPlaying;
            EventManager.Publish<OnPlayStateChangedMessage>(new() { IsPlaying = IsPlaying });
        }
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
        Time.timeScale = message.IsPlaying ? 1 : 0;
    }
}
