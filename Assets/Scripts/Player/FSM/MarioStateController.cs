using System;
using System.Collections;
using UnityEngine;

public enum MarioState {
    Default,
    SmallMario,
    SuperMario,
    FireMario,
    InvincibleSmallMario,
    DeadMario,
}

public class MarioStateController : StateController {
    public GameConstant consts;
    public PowerupType currentPowerupType = PowerupType.Default;
    public MarioState shouldBeNextState = MarioState.Default;

    public void Fire() {
        this.currentState.DoEventTriggeredActions(this, ActionType.Attack);
    }

    public override void Start() {
        base.Start();
        GameRestart(); // clear powerup in the beginning, go to start state
        ActionManager actionManager = GetComponent<ActionManager>();
        actionManager.fire.AddListener(Fire);
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

    // other methods
    public void SetRendererToFlicker() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        StartCoroutine(BlinkSpriteRenderer());
    }

    private IEnumerator BlinkSpriteRenderer() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        while (string.Equals(currentState.name, "InvincibleSmallMario", StringComparison.OrdinalIgnoreCase)) {
            // Toggle the visibility of the sprite renderer
            spriteRenderer.enabled = !spriteRenderer.enabled;
            animator.enabled = !animator.enabled;

            // Wait for the specified blink interval
            yield return new WaitForSeconds(consts.flickerInterval);
        }

        spriteRenderer.enabled = true;
        animator.enabled = true;
    }
}