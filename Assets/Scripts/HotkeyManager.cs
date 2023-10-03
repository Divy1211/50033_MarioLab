using UnityEngine;
using UnityEngine.InputSystem;

public class HotkeyManager : MonoBehaviour {
    private static Hotkey hotkey;

    void Start() {
        hotkey = new Hotkey();
        
        hotkey.Enable();
        hotkey.UiAction.Reset.performed += OnResetPress;
    }

    private void OnResetPress(InputAction.CallbackContext ctx) {
        GameManager.OnReset();
    }
}