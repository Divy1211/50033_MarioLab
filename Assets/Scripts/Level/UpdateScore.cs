using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour {
    private TextMeshProUGUI scoreText;

    private void OnScoreChange() {
        scoreText.text = "Score: " + GameManager.score;
    }

    void Start() {
        scoreText = GetComponent<TextMeshProUGUI>();

        GameManager.updateScoreEvent.AddListener(OnScoreChange);
    }
}