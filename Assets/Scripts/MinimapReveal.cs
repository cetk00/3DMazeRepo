using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapReveal : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] float cellSize = 3f;
    [SerializeField] MazeGenerator mazeGenerator;

    MazeCell[] allCells;
    HashSet<MinimapMarker> visitedMarkers = new HashSet<MinimapMarker>();

    void Start()
    {
        StartCoroutine(WaitForMaze());
    }

    IEnumerator WaitForMaze()
    {
        yield return new WaitUntil(() => mazeGenerator.IsGenerated);
        yield return null;

        allCells = FindObjectsByType<MazeCell>();
        Debug.Log("Found cells: " + allCells.Length);

        foreach (var cell in allCells)
        {
            var marker = cell.GetComponentInChildren<MinimapMarker>();
            if (marker != null)
                marker.SetVisible(false);
        }
    }

    void Update()
    {
        if (allCells == null) return;

        int x = Mathf.RoundToInt(player.position.x / cellSize);
        int z = Mathf.RoundToInt(player.position.z / cellSize);

        foreach (var cell in allCells)
        {
            int cx = Mathf.RoundToInt(cell.transform.position.x / cellSize);
            int cz = Mathf.RoundToInt(cell.transform.position.z / cellSize);

            if (cx == x && cz == z)
            {
                var marker = cell.GetComponentInChildren<MinimapMarker>();
                if (marker != null)
                {
                    marker.SetVisible(true);
                    visitedMarkers.Add(marker); // Track visited
                }
            }
        }
    }

    // Called when M is pressed — shows entire maze
    // Only reveals cells the player has already visited
    public void RevealAll()
    {
        if (allCells == null) return;
        foreach (var cell in allCells)
        {
            var marker = cell.GetComponentInChildren<MinimapMarker>();
            if (marker != null && visitedMarkers.Contains(marker))
                marker.SetVisible(true); // Only show visited ones
        }
    }

    // Called when M is released — hides unvisited cells again
    public void HideUnvisited()
    {
        if (allCells == null) return;
        foreach (var cell in allCells)
        {
            var marker = cell.GetComponentInChildren<MinimapMarker>();
            if (marker != null && !visitedMarkers.Contains(marker))
                marker.SetVisible(false); // Only hide unvisited ones
        }
    }
}