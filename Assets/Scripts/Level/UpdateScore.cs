using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour {
    public bool isHighscore;
    public bool isLife;

    private TextMeshProUGUI scoreText;

    private void OnScoreChange() {
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

    void Start() {
        scoreText = GetComponent<TextMeshProUGUI>();
        GameManager.updateScoreEvent.AddListener(OnScoreChange);
        OnScoreChange();
    }
}