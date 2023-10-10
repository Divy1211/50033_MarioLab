using UnityEngine;

public class CoinGen : MonoBehaviour {
    public bool spawnCoin = true;
    public Animator animator;
    public GameObject coinPrefab;
    public AudioSource audioSrc;

    private static readonly int isDeactivated = Animator.StringToHash("isDeactivated");

    private void OnReset() {
        gameObject.GetComponent<SpringJoint2D>().frequency = 5;
        animator.SetBool(isDeactivated, false);
    }
    void Start() {
        GameManager.resetEvent.AddListener(OnReset);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (GameManager.isGameInactive) {
            return;
        }

        if (col.gameObject.CompareTag("Coin") && !col.enabled) {
            audioSrc.PlayOneShot(audioSrc.clip);
            Destroy(col.gameObject);
            GameManager.score++;

            if (gameObject.CompareTag("QBox")) {
                gameObject.GetComponent<SpringJoint2D>().frequency = 0;
            }
        }

        // if a coin hits from below return also
        if (!spawnCoin || !col.enabled || (col.gameObject.CompareTag("Coin") && col.enabled)) {
            return;
        }

        if (!animator.GetBool(isDeactivated)) {
            animator.SetBool(isDeactivated, true);

            Vector3 pos = transform.position;
            GameObject coin = Instantiate(coinPrefab, new Vector3(pos.x, pos.y+1, pos.z), Quaternion.identity);
            coin.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 20, ForceMode2D.Impulse);
        }
    }
}