using UnityEngine;

public class Event : MonoBehaviour {
    public static GameEvent BackToMenu;
    public static GameEvent GameOver;
    public static GameEvent GameStart;
    public static GameEvent FastFwd;
    public static GameEvent HighScoreReset;
    public static GameEvent Pause;
    public static GameEvent PlayerHit;
    public static GameEvent Reset;
    public static GameEvent UpdateScore;

    public GameEvent _BackToMenu;
    public GameEvent _FastFwd;
    public GameEvent _GameOver;
    public GameEvent _GameStart;
    public GameEvent _HighScoreReset;
    public GameEvent _Pause;
    public GameEvent _PlayerHit;
    public GameEvent _Reset;
    public GameEvent _UpdateScore;

    private void Awake() {
        BackToMenu = _BackToMenu;
        FastFwd = _FastFwd;
        GameOver = _GameOver;
        GameStart = _GameStart;
        HighScoreReset = _HighScoreReset;
        Pause = _Pause;
        PlayerHit = _PlayerHit;
        Reset = _Reset;
        UpdateScore = _UpdateScore;
    }
}