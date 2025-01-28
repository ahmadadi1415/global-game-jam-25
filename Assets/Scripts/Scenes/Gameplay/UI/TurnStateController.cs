using System;
using TMPro;
using UnityEngine;

public class TurnStateController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnCountText;

    private void OnEnable()
    {
        EventManager.Subscribe<OnTurnChangedMessage>(OnTurnEnded);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OnTurnChangedMessage>(OnTurnEnded);
    }

    private void OnTurnEnded(OnTurnChangedMessage message)
    {
        _turnCountText.text = $"Your Turn: {message.CurrentTurn}";
    }
}