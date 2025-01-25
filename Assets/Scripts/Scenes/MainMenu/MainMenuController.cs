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
        SceneManager.LoadScene("Gameplay");
        UIAnimation.Pop(startBtn.gameObject);
    }
    void OnCreditClick()
    {
        
        creditPanel.SetActive(true);
        buttonPanel.SetActive(false);
        UIAnimation.Pop(creditBtn.gameObject);
        UIAnimation.ZoomIn(creditPanel, 0.5f);
        UIAnimation.ZoomOut(buttonPanel, 0.5f);
    }
    void OnClosePanel()
    {
        creditPanel.SetActive(false);
        buttonPanel.SetActive(true);
        UIAnimation.Pop(closeBtn.gameObject);
        UIAnimation.ZoomOut(creditPanel, 0.5f);
        UIAnimation.ZoomIn(buttonPanel, 0.5f);
    }
    void OnQuitClick()
    {  
        UIAnimation.Pop(quitBtn.gameObject);
        Application.Quit();
    }
}
