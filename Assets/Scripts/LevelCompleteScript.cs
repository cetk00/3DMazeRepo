using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;

public class LevelCompleteScript : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] TextMeshProUGUI finalTimeText;

    [Header("Script References")]
    [SerializeField] Timer timer;

    void Start()
    {
        levelCompletePanel.SetActive(false);
    }

    public void ShowLevelComplete()
    {
        timer.enabled = false;
        timer.HideTimer();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float elapsed = timer.GetElapsedTime();
        int minutes = Mathf.FloorToInt(elapsed / 60);
        int seconds = Mathf.FloorToInt(elapsed % 60);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        finalTimeText.text = "Time: " + formattedTime;

        levelCompletePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int next = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(next);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
