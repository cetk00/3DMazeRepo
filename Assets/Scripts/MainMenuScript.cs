using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [Header("Settings UI")]
    [SerializeField] TextMeshProUGUI modeButtonText; 

    private GameModeManager.GameMode selectedMode = GameModeManager.GameMode.CountUp;

    void Start()
    {
        int savedMode = PlayerPrefs.GetInt("GameMode", 0);
        selectedMode = (GameModeManager.GameMode)savedMode;
        UpdateModeButtonText();
    }

    public void ToggleMode()
    {
        if (selectedMode == GameModeManager.GameMode.CountUp)
            selectedMode = GameModeManager.GameMode.Countdown;
        else
            selectedMode = GameModeManager.GameMode.CountUp;

        PlayerPrefs.SetInt("GameMode", (int)selectedMode);
        PlayerPrefs.Save();

        UpdateModeButtonText();
    }

    void UpdateModeButtonText()
    {
        if (modeButtonText != null)
            modeButtonText.text = selectedMode == GameModeManager.GameMode.CountUp
                ? "Mode: Classic "
                : "Mode: Countdown ";
    }

    public void PlayGame()
    {
        GameModeManager.Instance.SetMode(selectedMode);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("You've left the game");
    }
}