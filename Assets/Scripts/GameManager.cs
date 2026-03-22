using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float nightDuration = 30f;
    private float timer;
    public TextMeshProUGUI timerText;

    public EnemySpawner spawner;
    public FadeManager fadeManager;

    public GameObject pauseMenuPanel;
    public GameObject youWinPanel;

    public TextMeshProUGUI nightText;
    public float textFadeDuration = 1f;

    public TextMeshProUGUI survivedText;
    private bool nightEnded = false;

    public AudioClip youSurvivedSound;

    void Start()
    {
        youWinPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        survivedText.gameObject.SetActive(false);

        SetNightDuration();

        Time.timeScale = 1f;
        StartCoroutine(ShowNightText());
    }

    [System.Obsolete]
    void Update()
    {
        if (nightEnded) return;

        timer -= Time.deltaTime;

        timerText.text = Mathf.Ceil(timer).ToString();

        if (timer <= 15f)
        {
            spawner.StopSpawning();
        }

        if (timer <= 0)
        {
            EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
            foreach (EnemyScript enemy in enemies)
            {
                enemy.enabled = false;
            }

            nightEnded = true;
            StartCoroutine(EndNightRoutine());
        }
    }

    void SetNightDuration()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Night 1")
        {
            nightDuration = 35f;
        }
        else if (sceneName == "Night 2")
        {
            nightDuration = 40f;
        }
        else if (sceneName == "Night 3")
        {
            nightDuration = 40f;
        }
        else
        {
            nightDuration = 35f;
        }

        timer = nightDuration;
    }

    IEnumerator EndNightRoutine()
    {
        survivedText.gameObject.SetActive(true);
        survivedText.text = SceneManager.GetActiveScene().name + " Survived";

        yield return StartCoroutine(FadeText(survivedText, true, 1f));  

        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(FadeText(survivedText, false, 1f));

        LoadNextScene();
    }

    IEnumerator FadeText(TextMeshProUGUI text, bool fadeIn, float duration)
    {
        float t = fadeIn ? 0 : duration;
        Color color = text.color;

        while (fadeIn ? t < duration : t > 0)
        {
            t += fadeIn ? Time.deltaTime : -Time.deltaTime;
            color.a = Mathf.Clamp01(t / duration);
            text.color = color;
            yield return null;
        }

        color.a = fadeIn ? 1f : 0f;
        text.color = color;
    }

    IEnumerator ShowNightText()
    {
        yield return StartCoroutine(FadeText(nightText, true, textFadeDuration)); 
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(FadeText(nightText, false, textFadeDuration)); 
    }

    void LoadNextScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        FadeManager fade = GetComponent<FadeManager>();

        if (currentScene == "Night 1")
        {
            SceneManager.LoadScene("Night 2");
        }
        else if (currentScene == "Night 2")
        {
            SceneManager.LoadScene("Night 3");
        }
        else
        {
            AudioManager.Instance.PlaySFX(youSurvivedSound);
            youWinPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }

    public void ReplayLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameData.remainingBullets = 45;
        SceneManager.LoadScene("Night 1");
    }

    public void HomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
