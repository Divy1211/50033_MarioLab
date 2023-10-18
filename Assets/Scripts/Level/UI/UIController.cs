using UnityEngine;

public class UIController : MonoBehaviour {
    public GameObject gameUi;
    public GameObject youDiedUi;
    public GameObject pausedUi;
    public GameObject loadingUi;

    public void OnGameOver(object _) {
        gameUi.SetActive(false);
        youDiedUi.SetActive(true);
    }

    public void OnPause(object isPausedObj) {
        bool isPaused = (bool)isPausedObj;

        gameUi.SetActive(!isPaused);
        pausedUi.SetActive(isPaused);
    }

    public void OnReset(object _) {
        gameUi.SetActive(true);
        youDiedUi.SetActive(false);
    }

    public void OnGameStart(object _) {
        loadingUi.SetActive(true);
    }
}