using System.Collections;
using UnityEngine;

public class PowerUpProc : MonoBehaviour {
    private AudioSource audioSrc;
    private Animator animator;
    private SpriteRenderer sprite;
    private ConstantForce2D force;
    private Rigidbody2D body;

    private bool goRight = true;

    void Start() {
        audioSrc = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        force = GetComponent<ConstantForce2D>();
        body = GetComponent<Rigidbody2D>();


        if (!CompareTag("Coin")) {
            force.force = new Vector2(0, -Physics2D.gravity.y*10);
            audioSrc.PlayOneShot(audioSrc.clip);
        }
    }


    private IEnumerator DestroyAfter(float timeSeconds) {
        yield return new WaitForSeconds(timeSeconds);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (LiveState.isGameInactive) {
            return;
        }

        if (col.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
            return;
        }

        if (!col.enabled && CompareTag("Coin") && (col.gameObject.CompareTag("Brick") || col.gameObject.CompareTag("QBox"))) {
            if(animator != null) {
                animator.enabled = false;
                sprite.enabled = false;
            }
            ++LiveState.score;
            audioSrc.PlayOneShot(audioSrc.clip);
            StartCoroutine(DestroyAfter(audioSrc.clip.length));
            return;
        }

        if(!col.gameObject.CompareTag("Ground") && !col.gameObject.CompareTag("QBox") && !col.gameObject.CompareTag("Brick")) {
            goRight = !goRight;
            force.force *= -1;
            body.velocity = Vector2.right * (goRight ? 1.5f : -1.5f);
        }
    }
}