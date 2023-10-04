using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    public static UnityEvent updateScoreEvent;
    public static UnityEvent resetEvent;
    public static UnityEvent killPlayerEvent;

    public static GameObject gameUi;
    public static GameObject youDiedUi;

    public static bool playerAlive = true;

    private static int _score;

    public static int score {
        get => _score;
        set {
            _score = value;
            updateScoreEvent.Invoke();
        }
    }

    private static Hotkey hotkey;
    private static AudioMixer masterMixer;
    private static bool fastForwarded;

    public static void OnReset() {
        gameUi.SetActive(true);
        youDiedUi.SetActive(false);
        score = 0;

        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coin in coins) {
            Destroy(coin);
        }

        resetEvent.Invoke();

        Time.timeScale = fastForwarded ? 2.0f : 1.0f;
    }

    public static void OnFastForward() {
        if (fastForwarded) {
            Time.timeScale = 1.0f;
            masterMixer.SetFloat("musicPitch", 1);
            masterMixer.SetFloat("musicSpeed", 1);
            fastForwarded = false;
            return;
        }
        Time.timeScale = 2.0f;
        masterMixer.SetFloat("musicPitch", 2);
        masterMixer.SetFloat("musicSpeed", 2);
        fastForwarded = true;
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
        killPlayerEvent = new UnityEvent();

        masterMixer = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;

        GameObject canvas = GameObject.Find("/Canvas");
        gameUi = canvas.transform.GetChild(0).gameObject;
        youDiedUi = canvas.transform.GetChild(1).gameObject;

        hotkey = new Hotkey();

        hotkey.Enable();
        hotkey.UiAction.Reset.performed += _ => OnReset();
        hotkey.UiAction.FastForward.performed += _ => OnFastForward();
    }
}