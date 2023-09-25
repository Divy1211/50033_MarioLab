using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 150;
    public float maxSpeed = 10;
    public float upSpeed = 30;
    public float deathImpulse = 30;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;
    public GameObject enemies;

    public JumpOverGoomba jumpOverGoomba;

    public GameObject gameUi;
    public GameObject youDiedUi;

    public AudioSource audioSrc;
    public AudioClip jumpSfx;
    public AudioClip deathSfx;

    public Animator marioAnimatior;

    public Transform cam;

    private bool onGroundState = true;
    private bool hasDoubleJumped = false;
    private bool releasedSpace = true;
    private bool marioAlive = true;

    private Vector3 marioStartingPos;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;

    private static readonly int isMarioIdle = Animator.StringToHash("isMarioIdle");
    private static readonly int isMarioRunning = Animator.StringToHash("isMarioRunning");
    private static readonly int isMarioAirborne = Animator.StringToHash("isMarioAirborne");
    private static readonly int onSkid = Animator.StringToHash("onSkid");
    private static readonly int onReset = Animator.StringToHash("onReset");


    void Start() {
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        marioStartingPos = marioBody.transform.position;
    }

    public void GameOver() {
        gameUi.SetActive(false);
        youDiedUi.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void ResetGame() {
        // reset ui
        gameUi.SetActive(true);
        youDiedUi.SetActive(false);
        // reset position
        if(!marioAlive) {
            marioAnimatior.SetTrigger(onReset);
        }
        marioBody.transform.position = marioStartingPos;
        marioBody.velocity = Vector2.zero;
        marioBody.angularVelocity = 0.0f;
        marioBody.rotation = 0.0f;
        marioSprite.flipX = false;
        marioAlive = true;
        upSpeed = 30;
        // reset score
        scoreText.text = "Score: 0";
        scoreText2.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform) {
            eachChild.transform.position = eachChild.GetComponent<EnemyMovement>().enemyStartingPos;
        }

        cam.position = new Vector3(0.0f, 0.2f, -10.0f);

        jumpOverGoomba.score = 0;
        audioSrc.Stop();
        audioSrc.Play();
    }

    public void RestartButtonCallback(int input) {
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            RestartButtonCallback(0);
        }

        if (!marioAlive) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f) {
                marioAnimatior.SetTrigger(onSkid);
            }
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f) {
                marioAnimatior.SetTrigger(onSkid);
            }
        }
    }

    void FixedUpdate() {
        if (!marioAlive) {
            return;
        }

        float moveH = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveH) > 0) {
            Vector2 movement = new Vector2(moveH, 0);
            if (marioBody.velocity.magnitude < maxSpeed) {
                marioBody.AddForce(movement * speed);
            }
        }

        marioAnimatior.SetBool(isMarioRunning, marioBody.velocity.magnitude > 0.10f);
        marioAnimatior.SetBool(isMarioIdle, !marioAnimatior.GetBool(isMarioRunning));
        marioAnimatior.SetBool(isMarioAirborne, !onGroundState);

        if (Input.GetKeyDown(KeyCode.Space) && releasedSpace) {
            releasedSpace = false;
            if (onGroundState || !hasDoubleJumped) {
                audioSrc.PlayOneShot(jumpSfx);
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                hasDoubleJumped = !onGroundState;
                if (onGroundState) {
                    upSpeed = 15;
                }
                onGroundState = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            releasedSpace = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground")) {
            onGroundState = true;
            hasDoubleJumped = false;
            upSpeed = 30;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy") && marioAlive) {

            marioAlive = false;
            marioAnimatior.Play("mario-die");
            marioBody.velocity = Vector2.zero;
            marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);

            audioSrc.Stop();
            audioSrc.PlayOneShot(deathSfx);
            // Debug.Log("Collided with goomba!");
        }
    }
}