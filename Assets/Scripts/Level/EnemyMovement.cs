using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    private const float MaxOffset = 5.0f;
    private const float EnemyPatrolTime = 4.0f;

    // private Vector2 velocity;
    private Rigidbody2D body;
    private Vector2 startingPos;


    private void OnReset() {
        body.position = startingPos;
        body.velocity = Vector2.left * MaxOffset/EnemyPatrolTime;
    }

    void Start() {
        body = GetComponent<Rigidbody2D>();
        startingPos = body.position;
        body.velocity = Vector2.left * MaxOffset/EnemyPatrolTime;

        GameManager.resetEvent.AddListener(OnReset);
    }

    void FixedUpdate() {
        if (Mathf.Abs(body.position.x - startingPos.x) >= MaxOffset) {
            body.velocity *= -1;
        }
    }
}