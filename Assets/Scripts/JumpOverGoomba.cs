using UnityEngine;
using TMPro;

public class JumpOverGoomba : MonoBehaviour {
    public Transform enemyLocation;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;
    public Vector2 boxSize = new Vector2(1, 8);
    public float maxDistance = 2;
    public LayerMask layerMask;

    private bool onGroundState;
    [System.NonSerialized] public int score = 0; // we don't want this to show up in the inspector
    private bool countScoreState = false;

    void FixedUpdate() {
        // mario jumps
        if (Input.GetKeyDown("space") && onGroundCheck()) {
            onGroundState = false;
            countScoreState = true;
        }

        // when jumping, and Goomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState) {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f) {
                countScoreState = false;
                score++;
                scoreText.text = "Score: " + score.ToString();
                scoreText2.text = "Score: " + score.ToString();
                // Debug.Log(score);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground"))
            onGroundState = true;
    }


    private bool onGroundCheck() {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask)) {
            // Debug.Log("on ground");
            return true;
        }
        // Debug.Log("not on ground");
        return false;
    }

    // void OnDrawGizmos() {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    // }
}