using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;

    [System.NonSerialized] public int moveRight = -1;
    [System.NonSerialized] public Vector2 enemyStartingPos;


    void Start() {
        enemyBody = GetComponent<Rigidbody2D>();

        // get the starting position
        originalX = transform.position.x;
        enemyStartingPos = enemyBody.position;
        // Debug.Log(enemyStartingPos);
        ComputeVelocity();
    }

    void ComputeVelocity() {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void Movegoomba() {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void Update() {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset) {
            // move goomba
            Movegoomba();
        }
        else {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    // void OnTriggerEnter2D(Collider2D other) {
    //     if (other.gameObject.CompareTag("Enemy")) {
    //         Debug.Log("Collided with goomba!");
    //     }
    // }
}