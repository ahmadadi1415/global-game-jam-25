using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonGroup : MonoBehaviour
{
    [SerializeField] private Color _buttonSelectedColor, _buttonUnselectedColor;
    [SerializeField] private List<Button> buttons; // Assign buttons in the inspector
    private Button selectedButton = null;          // Keeps track of the selected button

    private void Start()
    {
        foreach (Button button in buttons)
        {
            // Add a listener to each button
            button.onClick.AddListener(() => OnButtonClicked(button));
            button.onClick.AddListener(() => UIAnimation.Pop(button.gameObject));
            DeselectButton(button);
        }
    }

    private void OnEnable()
    {
        EventManager.Subscribe<OnTurnChangedMessage>(OnTurnChanged);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OnTurnChangedMessage>(OnTurnChanged);
    }

    private void OnTurnChanged(OnTurnChangedMessage message)
    {
        if (selectedButton != null) DeselectButton(selectedButton);
        selectedButton = null;
    }

    private void OnButtonClicked(Button clickedButton)
    {
        // If the clicked button is already selected, deselect it
        if (selectedButton == clickedButton)
        {
            DeselectButton(clickedButton);
            selectedButton = null;
        }
        else
        {
            // Deselect the current button, if any
            if (selectedButton != null)
                DeselectButton(selectedButton);

            // Select the new button
            SelectButton(clickedButton);
            selectedButton = clickedButton;
        }
    }

    private void SelectButton(Button button)
    {
        // Update button visuals for selected state
        var colors = button.colors;
        colors.normalColor = _buttonSelectedColor; // Example: make it green when selected
        button.colors = colors;
    }

    private void DeselectButton(Button button)
    {
        // Reset button visuals for normal state
        var colors = button.colors;
        colors.normalColor = _buttonUnselectedColor; // Example: revert to white
        button.colors = colors;
    }
}
