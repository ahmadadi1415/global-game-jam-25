using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickPowerUpUI : MonoBehaviour
{
    private Button _button;
    [SerializeField] private GameManager.PowerUp powerUp;
    public event Action OnClickPowerUpUI;
    public class OnClickPowerUIEvent
    {
        public GameManager.PowerUp powerUp;
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
