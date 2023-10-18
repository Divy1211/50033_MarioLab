using UnityEngine;

public class ButtonController : MonoBehaviour {
    public void OnClickResetGame() {
        Event.Reset.Raise(null);
    }
    public void OnClickPause() {
        Event.Pause.Raise(!LiveState.isPaused);
    }
    public void OnClickFastFwd() {
        Event.FastFwd.Raise(null);
    }
    public void OnClickResetHighScore() {
        Event.HighScoreReset.Raise(null);
    }
    public void OnClickStartGame() {
        Event.GameStart.Raise(null);
    }
    public void OnClickBackToMenu() {
        Event.BackToMenu.Raise(null);
    }
}