using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public bool mainMenu;

    public static GameObject gameUi;
    public static GameObject youDiedUi;
    public static GameObject pausedUi;
    public static GameObject loadingUi;
    public static ParticleSystem particleSys;

    public static bool isSuperMario;
    public static bool isUnkillable;
    public static bool isGameInactive => isPaused || !isPlayerAlive;
    public static GameStats stats;
    public static GameEvent ResetEvent;
    public static GameEvent UpdateScoreEvent;
    public static GameEvent PauseEvent;
    public static GameEvent PlayerHitEvent;

    public GameStats _Stats;
    public GameEvent _ResetEvent;
    public GameEvent _UpdateScoreEvent;
    public GameEvent _PauseEvent;
    public GameEvent _PlayerHitEvent;

    private static int _lives = 3;
    public static int lives {
        get => _lives;
        set {
            _lives = value;
            UpdateScoreEvent.Raise(null);
        }
    }

    private static int _score;
    public static int score {
        get => _score;
        set {
            _score = value;
            stats.highScore = Mathf.Max(stats.highScore, _score);
            UpdateScoreEvent.Raise(null);
        }
    }

    private static int startLives = lives;

    private static Hotkey hotkey;
    private static AudioMixer masterMixer;
    private static AudioSource musicSrc;

    public static bool isPlayerAlive = true;
    private static bool isPaused;
    private static bool isFastForwarded;

    private static void DestroyAllWithTag(string tag) {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objs) {
            Destroy(obj);
        }
    }

    public static void OnReset() {
        if (isPlayerAlive) {
            lives = startLives;
        } else {
            startLives = lives;
        }

        musicSrc.Play();
        isPaused = false;

        ResetEvent.Raise(null);

        gameUi.SetActive(true);
        youDiedUi.SetActive(false);

        score = 0;
        isPlayerAlive = true;
        isSuperMario = false;

        DestroyAllWithTag("Coin");
        DestroyAllWithTag("SuperShroom");
        DestroyAllWithTag("OneUpShroom");

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
        isPaused = !isPaused;
        gameUi.SetActive(!isPaused);
        pausedUi.SetActive(isPaused);
        if (isPaused) {
            musicSrc.Pause();
        } else {
            musicSrc.UnPause();
        }
        Time.timeScale = !isPaused ? isFastForwarded ? 2.0f : 1.0f : 0.0f;
        PauseEvent.Raise(isPaused);
    }

    public static void OnHighScoreReset() {
        stats.highScore = 0;
        UpdateScoreEvent.Raise(null);
    }

    public static void StartGame() {
        SceneManager.LoadSceneAsync("W1-1", LoadSceneMode.Single);
        loadingUi.SetActive(true);
    }

    public static void BackToMenu() {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

    public static void GameOver() {
        gameUi.SetActive(false);
        youDiedUi.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public static IEnumerator MakeKillable() {
        yield return new WaitForSeconds(1f);
        isUnkillable = false;
    }

    void Awake() {
        masterMixer = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;

        GameObject cardinal = GameObject.Find("/Cardinal");
        particleSys = cardinal.transform.GetChild(0).GetComponent<ParticleSystem>();
        musicSrc = cardinal.transform.GetChild(2).GetComponent<AudioSource>();

        GameObject ui = GameObject.Find("/UI");
        if(!mainMenu) {
            gameUi = ui.transform.GetChild(0).gameObject;
            youDiedUi = ui.transform.GetChild(1).gameObject;
            pausedUi = ui.transform.GetChild(2).gameObject;
        } else {
            loadingUi = ui.transform.GetChild(1).gameObject;
        }

        stats = _Stats;
        ResetEvent = _ResetEvent;
        UpdateScoreEvent = _UpdateScoreEvent;
        PauseEvent = _PauseEvent;
        PlayerHitEvent = _PlayerHitEvent;
    }

    private void Start() {
        startLives = lives;
    }
}