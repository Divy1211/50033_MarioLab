using UnityEngine;
using UnityEngine.InputSystem;

public class HotkeyManager : MonoBehaviour {
    private static Hotkey hotkey;

    void Start() {
        hotkey = new Hotkey();

        hotkey.Enable();
        hotkey.UiAction.Reset.performed += _ => GameManager.OnReset();
        hotkey.UiAction.FastForward.performed += _ => GameManager.OnFastForward();
    }

}