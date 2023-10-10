using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 150;
    public float maxSpeed = 10;
    public float upSpeed = 30;
    public float deathImpulse = 30;

    public AudioSource audioSrc;
    public AudioClip jumpSfx;

    public Animator marioAnimatior;

    private bool onGroundState = true;
    private bool hasDoubleJumped;

    private Vector3 marioStartingPos;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;

    private static readonly int isMarioIdle = Animator.StringToHash("isMarioIdle");
    private static readonly int isMarioRunning = Animator.StringToHash("isMarioRunning");
    private static readonly int isMarioAirborne = Animator.StringToHash("isMarioAirborne");
    private static readonly int onSkid = Animator.StringToHash("onSkid");
    private static readonly int onReset = Animator.StringToHash("onReset");

    private Vector2 dir = Vector2.zero;

    private void Jump() {
        if(GameManager.isGameInactive) {
            return;
        }
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

    private void Move(Vector2 dir) {
        if (GameManager.isGameInactive) {
            return;
        }
        this.dir = dir;
        FlipSprite(dir);
    }

    private void OnReset() {
        if(GameManager.isGameInactive) {
            marioAnimatior.SetTrigger(onReset);
        }
        marioBody.transform.position = marioStartingPos;
        marioBody.velocity = dir = Vector2.zero;
        marioBody.angularVelocity = 0.0f;
        marioSprite.flipX = false;
        upSpeed = 30;

        audioSrc.Play();
    }

    private void KillMario() {
        if (GameManager.isGameInactive) {
            return;
        }

        marioAnimatior.Play("mario-die");
        marioBody.velocity = Vector2.zero;
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void Start() {Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        ActionManager actionManager = GetComponent<ActionManager>();
        actionManager.jump.AddListener(Jump);
        actionManager.move.AddListener(Move);

        GameManager.resetEvent.AddListener(OnReset);
        GameManager.killPlayerEvent.AddListener(KillMario);
        marioStartingPos = marioBody.transform.position;
    }

    private void FlipSprite(Vector2 dir) {
        switch (dir.x) {
            case > 0: {
                marioSprite.flipX = false;
                if (marioBody.velocity.x < -0.05f) {
                    marioAnimatior.SetTrigger(onSkid);
                }
                return;
            }
            case < 0: {
                marioSprite.flipX = true;
                if (marioBody.velocity.x > 0.05f) {
                    marioAnimatior.SetTrigger(onSkid);
                }
                return;
            }
        }
    }

    private void KeepMoving() {
        if(marioBody.velocity.magnitude < maxSpeed) {
            marioBody.AddForce(new Vector2(dir.x, 0) * speed);
        }
    }

    private void Animate() {
        marioAnimatior.SetBool(isMarioRunning, marioBody.velocity.magnitude > 0.10f);
        marioAnimatior.SetBool(isMarioIdle, !marioAnimatior.GetBool(isMarioRunning));
        marioAnimatior.SetBool(isMarioAirborne, !onGroundState);
    }

    // called from mario's dying animation
    public void OnDeath() {
        GameManager.GameOver();
    }

    void Update() {
        if (GameManager.isGameInactive) {
            return;
        }
        Animate();
    }

    void FixedUpdate() {
        if(GameManager.isGameInactive) {
            return;
        }
        KeepMoving();
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("QBox") || col.gameObject.CompareTag("Brick")) {
            onGroundState = true;
            hasDoubleJumped = false;
            upSpeed = 30;
        }
    }
}