using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour {
     public UnityEvent<Vector2> move;
     public UnityEvent jump;

     private MarioAction marioAction;

     private void Start() {
          marioAction = new MarioAction();
          marioAction.Enable();

          marioAction.Player.Movement.performed += OnMovement;
          marioAction.Player.Movement.canceled += OnMovement;

          marioAction.Player.Jump.performed += OnJump;

          marioAction.Player.Jump.performed += OnJump;
     }

     private void OnMovement(InputAction.CallbackContext ctx) {
         move.Invoke(ctx.ReadValue<Vector2>());
     }

     private void OnJump(InputAction.CallbackContext ctx) {
         jump.Invoke();
     }

     private void OnReset() {
         GameManager.OnReset();
     }
}