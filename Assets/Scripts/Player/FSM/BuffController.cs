using UnityEngine;

public enum BuffState {
    Default,
    NormalMario,
    InvincibleMario,
}

public class BuffController : StateController {
    public GameConstant consts;
    public PowerupType currentPowerupType = PowerupType.Default;
    public BuffState shouldBeNextState = BuffState.Default;

    public override void Start() {
        base.Start();
        GameRestart(); // clear powerup in the beginning, go to start state
    }

    // this should be added to the GameRestart EventListener as callback
    public void GameRestart() {
        // clear powerup
        currentPowerupType = PowerupType.Default;
        // set the start state
        TransitionToState(startState);
    }

    public void SetPowerup(PowerupType i) {
        currentPowerupType = i;
    }

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // for visual aid to indicate which state this object is currently at
    public override void OnDrawGizmos() {
        if (currentState != null) {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(this.transform.position, 0.5f);
        }
    }
}