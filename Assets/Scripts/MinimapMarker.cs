using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        // Put this quad on the Minimap layer so only MinimapCamera sees it
        gameObject.layer = LayerMask.NameToLayer("Minimap");
        rend.enabled = false;
    }

    public void SetVisible(bool visible)
    {
        rend.enabled = visible;
    }
}