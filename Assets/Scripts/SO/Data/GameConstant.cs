using UnityEngine;

[CreateAssetMenu(fileName = "GameConstant", menuName = "ScriptableObjects/GameConstant", order = 1)]
public class GameConstant : ScriptableObject {
    public int maxLives;

    // Mario's movement
    public int speed;
    public int maxSpeed;
    public int upSpeed;
    public int deathImpulse;
    public Vector3 marioStartingPosition;
    public float flickerInterval;

    // Goomba's movement
    public float goombaPatrolTime;
    public float goombaMaxOffset;
}