using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;

    public enum GameMode { CountUp, Countdown }
    public GameMode currentMode = GameMode.CountUp;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persists between scenes
        }
        else Destroy(gameObject);
    }

    public void SetMode(GameMode mode) => currentMode = mode;
    public bool IsCountdown() => currentMode == GameMode.Countdown;
}
