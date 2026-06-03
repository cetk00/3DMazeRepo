using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlagScript : MonoBehaviour
{
    [SerializeField] LevelCompleteScript levelComplete;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            levelComplete.ShowLevelComplete();
        }
    }
}
