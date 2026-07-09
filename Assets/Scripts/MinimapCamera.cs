using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float height = 30f;

    bool isFollowing = true;

    void LateUpdate()
    {
        if (!isFollowing) return;

        transform.position = new Vector3(
            player.position.x,
            player.position.y + height,
            player.position.z
        );
    }

    public void StopFollowing() => isFollowing = false;
    public void StartFollowing() => isFollowing = true;
}