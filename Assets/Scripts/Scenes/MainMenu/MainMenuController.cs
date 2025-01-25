using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startBtn, creditBtn, quitBtn, closeBtn;
    [SerializeField] private GameObject buttonPanel, creditPanel;

    public AudioClip startClip;
    public AudioClip clickClip;
    public AudioClip hoverClip;

    [Header("Highlight Settings")]
    public Color hoverColor = Color.yellow;
    private Color originalColor;

    void Start()
    {

        originalColor = startBtn.GetComponent<Image>().color;

        startBtn.onClick.AddListener(() => StartCoroutine(OnStartClick()));
        creditBtn.onClick.AddListener(() => StartCoroutine(OnCreditClick()));
        closeBtn.onClick.AddListener(() => StartCoroutine(OnClosePanel()));
        quitBtn.onClick.AddListener(() => StartCoroutine(OnQuitClick()));

        AddHoverEffects(startBtn);
        AddHoverEffects(creditBtn);
        AddHoverEffects(quitBtn);
    }

    private IEnumerator PlayStartAudioAndWait()
    {
        if (startClip != null)
        {
            AudioSource.PlayClipAtPoint(startClip, Camera.main.transform.position, 0.25f);
            yield return new WaitForSeconds(startClip.length);
        }
    }

    private IEnumerator PlayClickAudioAndWait()
    {
        if (clickClip != null)
        {
            AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position, 0.25f);
            yield return new WaitForSeconds(clickClip.length);
        }
    }

    private void PlayHoverAudio()
    {
        if (hoverClip != null)
        {
            AudioSource.PlayClipAtPoint(hoverClip, Camera.main.transform.position, 0.25f);
        }
    }

    private IEnumerator OnStartClick()
    {
        yield return PlayStartAudioAndWait();
        SceneManager.LoadScene("Level 1");
    }

    private IEnumerator OnCreditClick()
    {
        yield return PlayClickAudioAndWait();
        creditPanel.SetActive(true);
        buttonPanel.SetActive(false);
    }

    private IEnumerator OnClosePanel()
    {
        yield return PlayClickAudioAndWait();
        creditPanel.SetActive(false);
        buttonPanel.SetActive(true);
    }

    private IEnumerator OnQuitClick()
    {
        yield return PlayClickAudioAndWait();
        Application.Quit();
    }

    private void AddHoverEffects(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry onEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        onEnter.callback.AddListener((data) =>
        {
            PlayHoverAudio();
            HighlightButton(button, true);
        });
        trigger.triggers.Add(onEnter);

        EventTrigger.Entry onExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        onExit.callback.AddListener((data) =>
        {
            HighlightButton(button, false);
        });
        trigger.triggers.Add(onExit);
    }

    private void HighlightButton(Button button, bool isHighlighted)
    {
        Image buttonImage = button.GetComponent<Image>();

        if (buttonImage != null)
        {
            if (isHighlighted)
            {
                buttonImage.color = hoverColor;
            }
            else
            {
                buttonImage.color = originalColor;
            }
        }
    }
}