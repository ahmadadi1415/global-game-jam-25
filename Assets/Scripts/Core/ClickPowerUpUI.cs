using System;
using UnityEngine;
using UnityEngine.UI;

public class ClickPowerUpUI : MonoBehaviour
{
    private Button _button;
    [SerializeField] private PowerUpType powerUp;
    public static event Action OnClickPowerUpUI;
    public class OnClickPowerUIEvent
    {
        public PowerUpType powerUp;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();   
    }

    private void Start()
    {
        _button.onClick.AddListener(() => {
            //var eventData = new OnClickPowerUIEvent { powerUp = powerUp };
            //OnClickPowerUIEvent?.Invoke(new OnClickPowerUIEvent { eventData });
            GameManager.Instance.SetPowerUp(powerUp);
        });
    }
}
