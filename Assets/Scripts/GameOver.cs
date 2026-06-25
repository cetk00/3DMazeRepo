using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}