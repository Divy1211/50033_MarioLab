using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour {
    public bool isHighscore;

    private TextMeshProUGUI scoreText;

    private void OnScoreChange() {
        if (!isHighscore) {
            scoreText.text = "Score: " + GameManager.score;
            return;
        }
        scoreText.text = "Top Score: " + GameManager.stats.highScore;
    }

    void Start() {
        scoreText = GetComponent<TextMeshProUGUI>();
        GameManager.updateScoreEvent.AddListener(OnScoreChange);
        OnScoreChange();
    }
}