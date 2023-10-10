using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    public static UnityEvent updateScoreEvent;
    public static UnityEvent resetEvent;
    public static UnityEvent killPlayerEvent;

    public static GameObject gameUi;
    public static GameObject youDiedUi;

    private static int _score;
    public static int score {
        get => _score;
        set {
            _score = value;
            updateScoreEvent.Invoke();
        }
    }

    public static bool isGameInactive => isPaused || !isPlayerAlive;

    private static Hotkey hotkey;
    private static AudioMixer masterMixer;

    private static bool isPlayerAlive = true;
    private static bool isPaused;
    private static bool isFastForwarded;

    public static void OnReset() {
        AudioListener.pause = false;
        isPaused = false;

        resetEvent.Invoke();

        gameUi.SetActive(true);
        youDiedUi.SetActive(false);

        score = 0;
        isPlayerAlive = true;

        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coin in coins) {
            Destroy(coin);
        }

        Time.timeScale = isFastForwarded ? 2.0f : 1.0f;
    }

    public static void OnFastForward() {
        if (isGameInactive) {
            return;
        }
        Time.timeScale = isFastForwarded ? 1.0f : 2.0f;
        masterMixer.SetFloat("musicPitch", isFastForwarded ? 1.0f : 2.0f);
        masterMixer.SetFloat("musicSpeed", isFastForwarded ? 1.0f : 2.0f);
        isFastForwarded = !isFastForwarded;
    }

    public static void OnPause() {
        if (!isPlayerAlive) {
            return;
        }
        AudioListener.pause = !isPaused;
        Time.timeScale = isPaused ? isFastForwarded ? 2.0f : 1.0f : 0.0f;
        isPaused = !isPaused;
    }

    public static void GameOver() {
        gameUi.SetActive(false);
        youDiedUi.SetActive(true);
        Time.timeScale = 0.0f;
    }

    void Awake() {
        updateScoreEvent = new UnityEvent();
        resetEvent = new UnityEvent();
        killPlayerEvent = new UnityEvent();

        masterMixer = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;

        GameObject canvas = GameObject.Find("/Canvas");
        gameUi = canvas.transform.GetChild(0).gameObject;
        youDiedUi = canvas.transform.GetChild(1).gameObject;
    }

    void Start() {
        killPlayerEvent.AddListener(() => isPlayerAlive = false);
    }
}