using UnityEngine;

public class PersistState : MonoBehaviour {
    public GameStats _Stats;

    public static int highScore {
        get => Stats.highScore;
        set => Stats.highScore = value;
    }

    private static GameStats Stats;

    private void Awake() {
        Stats = _Stats;
    }

    public static void OnHighScoreReset(object _) {
        highScore = 0;
        Event.UpdateScore.Raise(null);
    }
}