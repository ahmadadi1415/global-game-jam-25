using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip popClip;
    public AudioClip powerUpClip;
    public AudioClip powerUpUseClip;

    [SerializeField] private AudioSource _bubblePopSource, _powerUpSource;

    private void OnEnable()
    {
        EventManager.Subscribe<OnBubblePoppedMessage>(OnBubblePopped);
        EventManager.Subscribe<OnPowerUpUsedMessage>(OnPowerUpUsed);
    }


    private void OnDisable()
    {
        EventManager.Unsubscribe<OnBubblePoppedMessage>(OnBubblePopped);
        EventManager.Unsubscribe<OnPowerUpUsedMessage>(OnPowerUpUsed);
    }

    private void OnBubblePopped(OnBubblePoppedMessage message)
    {
        _bubblePopSource.PlayOneShot(popClip);
    }

    private void OnPowerUpUsed(OnPowerUpUsedMessage message)
    {
        _powerUpSource.PlayOneShot(powerUpUseClip);
    }
}