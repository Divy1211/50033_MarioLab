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
    public AudioClip starmanSfx;
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
        if(LiveState.isGameInactive) {
            return;
        }
        if (onGroundState || !hasDoubleJumped) {
            audioSrc.PlayOneShot(LiveState.isSuperMario ? jumpSuperSfx : jumpSfx);
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            hasDoubleJumped = !onGroundState;
            if (onGroundState) {
                upSpeed = 15;
            }
            onGroundState = false;
        }
    }

    private void Move(Vector2 dir) {
        if (LiveState.isGameInactive) {
            return;
        }
        this.dir = dir;
        FlipSprite(dir);
    }

    public void OnReset(object _) {
        if(LiveState.isPaused) {
            return;
        }

        MarioStateController m = GetComponent<MarioStateController>();
        m.TransitionToState(m.startState);

        BuffController b = GetComponent<BuffController>();
        b.TransitionToState(b.startState);

        marioAnimatior.SetTrigger(onReset);
        marioBody.transform.position = consts.marioStartingPosition;
        marioBody.velocity = dir = Vector2.zero;
        marioBody.angularVelocity = 0.0f;
        marioSprite.flipX = false;
        upSpeed = 30;

        audioSrc.Play();
    }

    public void KillMario(object isDeadObj) {
        bool isDead = (bool)isDeadObj;

        if (LiveState.isGameInactive) {
            return;
        }

        LiveState.isPlayerAlive = !isDead;
        GetComponent<MarioStateController>().SetPowerup(PowerupType.Damage);

        if (!isDead) {
            LiveState.isSuperMario = false;
            LiveState.isFireMario = false;
            audioSrc.PlayOneShot(dmgSfx);
            return;
        }

        marioAnimatior.Play("mario-die");
        marioBody.velocity = Vector2.zero;
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
        --LiveState.lives;
    }

    public void OnPause(object pausedObj) {
        bool paused = (bool)pausedObj;
        if (!paused) {
            Move(Vector2.zero);
            return;
        }
        audioSrc.PlayOneShot(pauseSfx);
    }

    private void Start() {
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
        Event.GameOver.Raise(null);
    }

    private void Update() {
        if (LiveState.isGameInactive) {
            return;
        }
        Animate();
    }

    private void FixedUpdate() {
        if(LiveState.isGameInactive) {
            return;
        }
        KeepMoving();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("QBox") || col.gameObject.CompareTag("Brick")) {
            onGroundState = true;
            hasDoubleJumped = false;
            upSpeed = 30;
        }

        if (!LiveState.isPlayerAlive) {
            return;
        }

        switch (col.gameObject.tag) {
            // yes, once again, these state related checks should by right be done through the FSMs... :')
            case "SuperShroom": {
                GetComponent<MarioStateController>().SetPowerup(PowerupType.MagicMushroom);
                LiveState.isSuperMario = true;
                audioSrc.PlayOneShot(superSfx);
                break;
            }
            case "FireFlower": {
                GetComponent<MarioStateController>().SetPowerup(PowerupType.FireFlower);
                LiveState.isSuperMario = true;
                LiveState.isFireMario = true;
                audioSrc.PlayOneShot(superSfx);
                break;
            }
            case "StarMan": {
                GetComponent<BuffController>().SetPowerup(PowerupType.StarMan);
                LiveState.isStarman = true;
                LiveState.musicSrc.Stop();
                LiveState.musicSrc.PlayOneShot(starmanSfx);
                StartCoroutine(LiveState.MakeKillable(10f));
                audioSrc.PlayOneShot(superSfx);
                break;
            }
            case "OneUpShroom": {
                audioSrc.PlayOneShot(lifeSfx);
                ++LiveState.lives;
                break;
            }
        }
    }
}