using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour {
    public bool isHighscore;
    public bool isLife;

    private TextMeshProUGUI scoreText;

    public void OnScoreChange(object _) {
        if (isLife) {
            scoreText.text = "Lives: " + LiveState.lives;
            return;
        }

        if (isHighscore) {
            scoreText.text = "Top Score: " + PersistState.highScore;
            return;
        }

        scoreText.text = "Score: " + LiveState.score;
    }

    private void OnEnable() {
        scoreText = GetComponent<TextMeshProUGUI>();
        OnScoreChange(null);
    }
}