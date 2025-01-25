using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startBtn, creditBtn, quitBtn, closeBtn;
    [SerializeField] private GameObject buttonPanel, creditPanel;

    void Start()
    {
        startBtn.onClick.AddListener(OnStartClick);
        creditBtn.onClick.AddListener(OnCreditClick);
        closeBtn.onClick.AddListener(OnClosePanel);
        quitBtn.onClick.AddListener(OnQuitClick);
    }
    void OnStartClick()
    {
        SceneManager.LoadScene("Level 1");
    }
    void OnCreditClick()
    {
        creditPanel.SetActive(true);
        buttonPanel.SetActive(false);
    }
    void OnClosePanel()
    {
        creditPanel.SetActive(false);
        buttonPanel.SetActive(true);
    }
    void OnQuitClick()
    {  
        Application.Quit();
    }
}
