using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LiveState : MonoBehaviour {
    // this is technically still a singleton, but now it only stores values that do not persist

    public static ParticleSystem particleSys;

    // yes, these 5 states specific to mario should by right be in the FSMs... :')
    public static bool isSuperMario;
    public static bool isFireMario;
    public static bool isStarman;
    public static bool isUnkillable;
    public static bool isPlayerAlive = true;

    public static bool isPaused;
    public static bool isFastForwarded;
    public static bool isGameInactive => isPaused || !isPlayerAlive;

    public static AudioSource musicSrc;

    private static int _lives = 3;
    public static int lives {
        get => _lives;
        set {
            _lives = value;
            Event.UpdateScore.Raise(null);
        }
    }

    private static int _score;
    public static int score {
        get => _score;
        set {
            _score = value;
            PersistState.highScore = Mathf.Max(PersistState.highScore, _score);
            Event.UpdateScore.Raise(null);
        }
    }

    private static int startLives = lives;

    private static Hotkey hotkey;
    private static AudioMixer masterMixer;

    private static void DestroyAllWithTag(string tag) {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objs) {
            Destroy(obj);
        }
    }

    public static void OnReset(object _) {
        if (isPlayerAlive) {
            lives = startLives;
        } else {
            startLives = lives;
        }

        musicSrc.Play();
        isPaused = false;

        score = 0;
        isPlayerAlive = true;
        isSuperMario = false;
        isFireMario = false;
        isStarman = false;

        DestroyAllWithTag("Coin");
        DestroyAllWithTag("SuperShroom");
        DestroyAllWithTag("OneUpShroom");
        DestroyAllWithTag("FireFlower");
        DestroyAllWithTag("StarMan");

        Time.timeScale = isFastForwarded ? 2.0f : 1.0f;
    }

    public static void OnFastForward(object _) {
        if (isGameInactive) {
            return;
        }
        Time.timeScale = isFastForwarded ? 1.0f : 2.0f;
        masterMixer.SetFloat("musicPitch", isFastForwarded ? 1.0f : 2.0f);
        masterMixer.SetFloat("musicSpeed", isFastForwarded ? 1.0f : 2.0f);
        isFastForwarded = !isFastForwarded;
    }

    public static void OnPause(object _) {
        if (!isPlayerAlive) {
            return;
        }
        isPaused = !isPaused;
        if (isPaused) {
            musicSrc.Pause();
        } else {
            musicSrc.UnPause();
        }
        Time.timeScale = !isPaused ? isFastForwarded ? 2.0f : 1.0f : 0.0f;
    }

    public static void OnGameStart(object _) {
        SceneManager.LoadSceneAsync("W1-1", LoadSceneMode.Single);
    }

    public static void OnBackToMenu(object _) {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

    public static void OnGameOver(object _) {
        Time.timeScale = 0.0f;
    }

    public static IEnumerator MakeKillable(float time) {
        yield return new WaitForSeconds(time);
        isUnkillable = false;
        if (isStarman) {
            musicSrc.Stop();
            musicSrc.Play();
        }
        isStarman = false;
    }

    void Awake() {
        masterMixer = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;

        GameObject cardinal = GameObject.Find("/Cardinal");
        musicSrc = cardinal.transform.GetChild(0).GetComponent<AudioSource>();
        particleSys = cardinal.transform.GetChild(1).GetComponent<ParticleSystem>();
    }
}