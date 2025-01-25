using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteAllLevelController : MonoBehaviour
{
    [SerializeField] private Button quitBtn;
    void Start()
    {
        quitBtn.onClick.AddListener(OnQuitClick);
    }

    private void OnQuitClick()
    {
        // scene manager to main menu
    }
}
