using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public GameConstant consts;

    private float speed;
    private float maxSpeed;
    private float upSpeed;
    private float deathImpulse;

    public AudioSource audioSrc;
    public AudioClip jumpSfx;
    public AudioClip jumpSuperSfx;
    public AudioClip superSfx;
    public AudioClip dmgSfx;
    public AudioClip lifeSfx;
    public AudioClip pauseSfx;

    public Animator marioAnimatior;

    private bool onGroundState = true;
    private bool hasDoubleJumped;

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
            audioSrc.PlayOneShot(GameManager.isSuperMario ? jumpSuperSfx : jumpSfx);
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

        SetSize(1.0f);
        marioBody.transform.position = consts.marioStartingPosition;
        marioBody.velocity = dir = Vector2.zero;
        marioBody.angularVelocity = 0.0f;
        marioSprite.flipX = false;
        upSpeed = 30;

        audioSrc.Play();
    }

    private void SetSize(float s) {
        transform.localScale = new Vector3(s, s, 1.0f);
    }

    private void KillMario(bool isDead) {
        if (GameManager.isGameInactive) {
            return;
        }

        if (!isDead) {
            GameManager.isSuperMario = false;
            SetSize(1.0f);
            audioSrc.PlayOneShot(dmgSfx);
            return;
        }

        marioAnimatior.Play("mario-die");
        marioBody.velocity = Vector2.zero;
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
        --GameManager.lives;
    }

    private void OnPause(bool paused) {
        if (!paused) {
            Move(Vector2.zero);
            return;
        }
        audioSrc.PlayOneShot(pauseSfx);
    }

    void Start() {
        Application.targetFrameRate = 30;

        speed = consts.speed;
        maxSpeed = consts.maxSpeed;
        upSpeed = consts.upSpeed;
        deathImpulse = consts.deathImpulse;

        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        ActionManager actionManager = GetComponent<ActionManager>();
        actionManager.jump.AddListener(Jump);
        actionManager.move.AddListener(Move);

        GameManager.resetEvent.AddListener(OnReset);
        GameManager.playerHitEvent.AddListener(KillMario);
        GameManager.pauseEvent.AddListener(OnPause);
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

        if (col.gameObject.CompareTag("SuperShroom")) {
            GameManager.isSuperMario = true;
            audioSrc.PlayOneShot(superSfx);
            SetSize(1.5f);
        }
        if (col.gameObject.CompareTag("OneUpShroom")) {
            audioSrc.PlayOneShot(lifeSfx);
            ++GameManager.lives;
            GameManager.updateScoreEvent.Invoke();
        }
    }
}