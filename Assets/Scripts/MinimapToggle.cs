using UnityEngine;
using TMPro;

public class MinimapToggle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject minimapDisplay;
    [SerializeField] GameObject minimapCamera;
    [SerializeField] TextMeshProUGUI buttonText;

    [Header("Full Map")]
    [SerializeField] MinimapReveal minimapReveal;
    [SerializeField] Camera mapCamera;
    [SerializeField] MinimapCamera minimapCameraScript;

    [Header("Maze Settings")]
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] float cellSize = 3f;

    [Header("Player")]
    [SerializeField] PlayerMovement playerMovementScript;

    bool minimapEnabled = false;
    bool fullMapOpen = false;
    float originalSize;
    Vector3 originalCameraPosition;

    void Start()
    {
        minimapDisplay.SetActive(false);
        minimapCamera.SetActive(false);
        originalSize = mapCamera.orthographicSize;
        originalCameraPosition = mapCamera.transform.position;
        UpdateButtonText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            ToggleFullMap();
    }

    public void ToggleMinimap()
    {
        if (fullMapOpen)
        {
            CloseFullMap();
            return;
        }

        minimapEnabled = !minimapEnabled;
        minimapDisplay.SetActive(minimapEnabled);
        minimapCamera.SetActive(minimapEnabled);

        PlayerPrefs.SetInt("MinimapEnabled", minimapEnabled ? 1 : 0);
        PlayerPrefs.Save();

        UpdateButtonText();
    }

    void ToggleFullMap()
    {
        if (fullMapOpen)
            CloseFullMap();
        else
            OpenFullMap();
    }

    void OpenFullMap()
    {
        fullMapOpen = true;

        // Disable player movement and show cursor
        playerMovementScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Stop camera following player
        minimapCameraScript.StopFollowing();

        minimapDisplay.SetActive(true);
        minimapCamera.SetActive(true);

        // Only show visited cells
        minimapReveal.RevealAll();

        // Center camera on entire maze
        float mazeWorldWidth = (mazeGenerator.MazeWidth - 1) * cellSize;
        float mazeWorldDepth = (mazeGenerator.MazeDepth - 1) * cellSize;
        float centerX = mazeWorldWidth / 2f;
        float centerZ = mazeWorldDepth / 2f;

        mapCamera.transform.position = new Vector3(
            centerX,
            mapCamera.transform.position.y,
            centerZ
        );

        // Fit entire maze in frame
        float mazeWorldSize = Mathf.Max(mazeWorldWidth, mazeWorldDepth);
        mapCamera.orthographicSize = (mazeWorldSize / 2f) + cellSize;

        // Make minimap fullscreen
        RectTransform rt = minimapDisplay.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.1f, 0.1f);
        rt.anchorMax = new Vector2(0.9f, 0.9f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        UpdateButtonText();
    }

    void CloseFullMap()
    {
        fullMapOpen = false;

        // Re-enable player movement and hide cursor
        playerMovementScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Resume camera following player
        minimapCameraScript.StartFollowing();

        minimapReveal.HideUnvisited();
        mapCamera.orthographicSize = originalSize;

        // Restore minimap to corner
        RectTransform rt = minimapDisplay.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(1f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.offsetMin = new Vector2(-220f, -220f);
        rt.offsetMax = new Vector2(-20f, -20f);

        // Hide minimap if it was off before
        if (!minimapEnabled)
        {
            minimapDisplay.SetActive(false);
            minimapCamera.SetActive(false);
        }

        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        if (buttonText == null) return;
        if (fullMapOpen)
            buttonText.text = "Map: FULL [M]";
        else
            buttonText.text = minimapEnabled ? "Map: ON" : "Map: OFF";
    }
}