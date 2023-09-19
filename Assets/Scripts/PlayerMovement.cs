using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 150;
    public float maxSpeed = 10;
    public float upSpeed = 25;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;
    public GameObject enemies;

    public JumpOverGoomba jumpOverGoomba;

    public GameObject gameUi;
    public GameObject youDiedUi;

    public AudioSource audioSrc;
    public AudioClip jumpSfx;
    public AudioClip deathSfx;

    private bool onGroundState = true;
    [System.NonSerialized] public Vector3 marioStartingPos;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;


    void Start() {
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        marioStartingPos = marioBody.transform.position;
    }

    private void ResetGame() {
        // reset ui
        gameUi.SetActive(true);
        youDiedUi.SetActive(false);
        // reset position
        marioBody.transform.position = marioStartingPos;
        marioBody.velocity = Vector2.zero;
        marioBody.angularVelocity = 0.0f;
        marioBody.rotation = 0.0f;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        scoreText2.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform) {
            eachChild.transform.position = eachChild.GetComponent<EnemyMovement>().enemyStartingPos;
        }

        jumpOverGoomba.score = 0;
        audioSrc.Stop();
        audioSrc.Play();
    }

    public void RestartButtonCallback(int input) {
        // Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            RestartButtonCallback(0);
        }
        if (Time.timeScale == 0.0f) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            marioSprite.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            marioSprite.flipX = false;
        }
    }

    void FixedUpdate() {
        float moveH = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveH) > 0) {
            Vector2 movement = new Vector2(moveH, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGroundState) {
            audioSrc.PlayOneShot(jumpSfx);
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground"))
            onGroundState = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            gameUi.SetActive(false);
            youDiedUi.SetActive(true);
            Time.timeScale = 0.0f;
            audioSrc.Stop();
            audioSrc.PlayOneShot(deathSfx);
            // Debug.Log("Collided with goomba!");
        }
    }
}