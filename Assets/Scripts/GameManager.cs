using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    public static UnityEvent updateScoreEvent, resetEvent;
    public static GameObject gameUi, youDiedUi;

    private static int _score;
    public static int score {
        get => _score;
        set {
            _score = value;
            updateScoreEvent.Invoke();
        }
    }

    private static Hotkey hotkey;

    public static void OnReset() {
        gameUi.SetActive(true);
        youDiedUi.SetActive(false);
        score = 0;
        resetEvent.Invoke();
        Time.timeScale = 1.0f;
    }

    // called from mario's dying animation
    public static void GameOver() {
        gameUi.SetActive(false);
        youDiedUi.SetActive(true);
        Time.timeScale = 0.0f;
    }

    void Awake() {
        updateScoreEvent = new UnityEvent();
        resetEvent = new UnityEvent();

        GameObject canvas = GameObject.Find("/Canvas");
        gameUi = canvas.transform.GetChild(0).gameObject;
        youDiedUi = canvas.transform.GetChild(1).gameObject;

        hotkey = new Hotkey();

        hotkey.Enable();
        hotkey.UiAction.Reset.performed += OnResetPress;
    }

    private void OnResetPress(InputAction.CallbackContext ctx) {
        OnReset();
    }
}