using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowMarker : MonoBehaviour
{
    [Header("Throw Settings")]
    [SerializeField] GameObject markerPrefab;   // Your Marker sphere prefab
    [SerializeField] Transform throwOrigin;     // Assign your camera/player head
    [SerializeField] float throwForce = 10f;
    [SerializeField] int maxMarkers = 3;        // Max markers allowed at once

    [Header("UI")]
    [SerializeField] TextMeshProUGUI markerCountText; // Shows "Markers: 2/3"

    private Queue<GameObject> spawnedMarkers = new Queue<GameObject>();

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Press E or left mouse button to throw
        if (Input.GetKeyDown(KeyCode.E))
        {
            ThrowMarkerObject();
        }

        // Press Q to pick up / remove oldest marker
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RemoveOldestMarker();
        }
    }

    void ThrowMarkerObject()
    {
        // Remove oldest if at max
        if (spawnedMarkers.Count >= maxMarkers)
            RemoveOldestMarker();

        // Spawn and throw
        GameObject marker = Instantiate(
            markerPrefab,
            throwOrigin.position + throwOrigin.forward,
            Quaternion.identity
        );

        Rigidbody rb = marker.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(throwOrigin.forward * throwForce, ForceMode.Impulse);

        spawnedMarkers.Enqueue(marker);
        UpdateUI();
    }

    void RemoveOldestMarker()
    {
        if (spawnedMarkers.Count > 0)
        {
            GameObject oldest = spawnedMarkers.Dequeue();
            if (oldest != null)
                Destroy(oldest);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (markerCountText != null)
            markerCountText.text = "         " + spawnedMarkers.Count + "/" + maxMarkers;
    }
}