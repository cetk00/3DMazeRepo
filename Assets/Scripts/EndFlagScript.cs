using UnityEngine;

public class EndFlagScript : MonoBehaviour
{
    private LevelCompleteScript levelComplete;

    void Start()
    {
        // true = search inactive GameObjects too
        levelComplete = FindObjectOfType<LevelCompleteScript>(true);

        if (levelComplete == null)
            Debug.LogError("LevelCompleteScript not found in scene!");
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            levelComplete.ShowLevelComplete();
        }
    }
}