using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public string nextSceneName;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        }
    }
}