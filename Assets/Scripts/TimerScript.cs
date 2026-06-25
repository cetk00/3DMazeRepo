using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float countdownStartTime = 180f; // 3 minutes
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color warningColor = Color.red;

    [Header("References")]
    [SerializeField] GameOver gameOver;

    float elapsedTime;
    float timeRemaining;
    bool isRunning = true;
    bool isCountdown;

    void Start()
    {
        isCountdown = GameModeManager.Instance.IsCountdown();
        timeRemaining = countdownStartTime;
    }

    void Update()
    {
        if (!isRunning) return;

        if (isCountdown)
            UpdateCountdown();
        else
            UpdateCountUp();
    }

    void UpdateCountUp()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateCountdown()
    {
        timeRemaining -= Time.deltaTime;
        elapsedTime += Time.deltaTime;

        if (timeRemaining <= 10f)
            timerText.color = Mathf.Sin(Time.time * 10f) > 0 ? warningColor : normalColor;
        else
            timerText.color = normalColor;

        timeRemaining = Mathf.Clamp(timeRemaining, 0f, countdownStartTime);

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeRemaining <= 0f)
        {
            isRunning = false;
            gameOver.ShowGameOver();
        }
    }

    public void StopTimer() => isRunning = false;
    public void HideTimer() => timerText.gameObject.SetActive(false);

    public void AddTime(float amount)
    {
        timeRemaining = Mathf.Clamp(timeRemaining + amount, 0f, countdownStartTime);
    }

    public float GetElapsedTime() => elapsedTime;
    public float GetTimeRemaining() => timeRemaining;
    public bool GetIsCountdown() => isCountdown;
}