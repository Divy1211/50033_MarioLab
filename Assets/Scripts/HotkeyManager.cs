using UnityEngine;

public class HotkeyManager : MonoBehaviour {
    private static Hotkey hotkey;

    void Awake() {
        hotkey = new Hotkey();

        hotkey.Enable();
        hotkey.UiAction.Reset.performed += _ => Event.Reset.Raise(null);
        hotkey.UiAction.FastForward.performed += _ => Event.FastFwd.Raise(null);
        hotkey.UiAction.Pause.performed += _ => Event.Pause.Raise(!LiveState.isPaused);
    }

}