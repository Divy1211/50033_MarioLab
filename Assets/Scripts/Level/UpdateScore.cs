using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour {
    public bool isHighscore;
    public bool isLife;

    private TextMeshProUGUI scoreText;

    public void OnScoreChange(object _) {
        if (isLife) {
            scoreText.text = "Lives: " + GameManager.lives;
            return;
        }

        if (isHighscore) {
            scoreText.text = "Top Score: " + GameManager.stats.highScore;
            return;
        }

        scoreText.text = "Score: " + GameManager.score;
    }

    private void OnEnable() {
        scoreText = GetComponent<TextMeshProUGUI>();
        OnScoreChange(null);
    }
}