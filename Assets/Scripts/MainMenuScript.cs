using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject modeMenu;

    [Header("Settings UI")]
    [SerializeField] TextMeshProUGUI modeButtonText;
    [SerializeField] TMP_InputField mazeSizeInput;

    [Header("Maze Size")]
    [SerializeField] int minSize = 5;
    [SerializeField] int maxSize = 50;
    [SerializeField] int defaultSize = 10;

    private GameModeManager.GameMode selectedMode = GameModeManager.GameMode.CountUp;

    void Start()
    {
        int savedMode = PlayerPrefs.GetInt("GameMode", 0);
        selectedMode = (GameModeManager.GameMode)savedMode;
        UpdateModeButtonText();

        // Load last used maze size
        int savedSize = PlayerPrefs.GetInt("MazeSize", defaultSize);
        mazeSizeInput.text = savedSize.ToString();
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

    public void OpenModeMenu()
    {
        mainMenu.SetActive(false);
        modeMenu.SetActive(true);
        mazeSizeInput.gameObject.SetActive(false);
    }

    public void CloseModeMenu()
    {
        modeMenu.SetActive(false);
        mainMenu.SetActive(true);
        mazeSizeInput.gameObject.SetActive(true);
    }

    public void PlayGame()
    {
        // Parse and clamp maze size
        int size = defaultSize;
        if (int.TryParse(mazeSizeInput.text, out int parsed))
            size = Mathf.Clamp(parsed, minSize, maxSize);

        // Save size for MazeGenerator to read
        PlayerPrefs.SetInt("MazeSize", size);
        PlayerPrefs.Save();

        GameModeManager.Instance.SetMode(selectedMode);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("You've left the game");
    }
}