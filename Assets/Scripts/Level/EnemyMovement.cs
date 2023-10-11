using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public Animator goombaAnimatior;
    public AudioSource audioSrc;

    public GameConstant consts;

    private float maxOffset = 5.0f;
    private float patrolTime = 4.0f;

    // private Vector2 velocity;
    private Rigidbody2D goombaBody;
    private SpriteRenderer goombaSprite;
    private BoxCollider2D goombaCollider;
    private Vector2 startingPos;

    private void OnReset() {
        goombaBody.position = startingPos;
        goombaBody.velocity = Vector2.left * maxOffset/patrolTime;
        goombaSprite.enabled = true;
        goombaCollider.enabled = true;
        goombaAnimatior.enabled = true;
        goombaAnimatior.Play("goomba-walk");
    }

    // called from goomba's dying animation
    public void OnDeath() {
        goombaAnimatior.enabled = false;
        goombaSprite.enabled = false;
    }

    void Start() {
        maxOffset = consts.goombaMaxOffset;
        patrolTime = consts.goombaPatrolTime;

        goombaBody = GetComponent<Rigidbody2D>();
        goombaSprite = GetComponent<SpriteRenderer>();
        goombaCollider = GetComponent<BoxCollider2D>();
        startingPos = goombaBody.position;
        goombaBody.velocity = Vector2.left * maxOffset/patrolTime;

        GameManager.resetEvent.AddListener(OnReset);
    }

    void FixedUpdate() {
        if (Mathf.Abs(goombaBody.position.x - startingPos.x) >= maxOffset) {
            goombaBody.velocity *= -1;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (GameManager.isUnkillable || GameManager.isGameInactive || !col.gameObject.CompareTag("Player")) {
            return;
        }
        if(!col.enabled) {
            GameManager.playerHitEvent.Invoke(!GameManager.isSuperMario);
            GameManager.isUnkillable = true;
            StartCoroutine(GameManager.MakeKillable());
            return;
        }

        ++GameManager.score;
        goombaBody.velocity = Vector2.zero;
        goombaCollider.enabled = false;
        goombaAnimatior.Play("goomba-die");
        audioSrc.PlayOneShot(audioSrc.clip);
    }
}