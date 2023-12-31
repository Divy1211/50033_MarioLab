//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputSys/Hotkey.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Hotkey: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Hotkey()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Hotkey"",
    ""maps"": [
        {
            ""name"": ""UiAction"",
            ""id"": ""a30baaa9-7dea-4748-addd-93dc0dc4b8fb"",
            ""actions"": [
                {
                    ""name"": ""Reset"",
                    ""type"": ""Button"",
                    ""id"": ""24633939-5a58-44b9-98b4-35e0a301dfa9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FastForward"",
                    ""type"": ""Button"",
                    ""id"": ""69b9e741-5f1d-497f-8a4b-bd1c47659806"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""b3e580fd-6638-4cb5-989f-9b38a976dbe8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6226b2eb-a0e9-44c8-9415-623ed4cd2c09"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a19f98ef-082c-4cd6-92a5-e0a76e6d9b7f"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FastForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f9b6e30-20ef-46b5-a9aa-250ee327c461"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": []
        }
    ]
}");
        // UiAction
        m_UiAction = asset.FindActionMap("UiAction", throwIfNotFound: true);
        m_UiAction_Reset = m_UiAction.FindAction("Reset", throwIfNotFound: true);
        m_UiAction_FastForward = m_UiAction.FindAction("FastForward", throwIfNotFound: true);
        m_UiAction_Pause = m_UiAction.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // UiAction
    private readonly InputActionMap m_UiAction;
    private List<IUiActionActions> m_UiActionActionsCallbackInterfaces = new List<IUiActionActions>();
    private readonly InputAction m_UiAction_Reset;
    private readonly InputAction m_UiAction_FastForward;
    private readonly InputAction m_UiAction_Pause;
    public struct UiActionActions
    {
        private @Hotkey m_Wrapper;
        public UiActionActions(@Hotkey wrapper) { m_Wrapper = wrapper; }
        public InputAction @Reset => m_Wrapper.m_UiAction_Reset;
        public InputAction @FastForward => m_Wrapper.m_UiAction_FastForward;
        public InputAction @Pause => m_Wrapper.m_UiAction_Pause;
        public InputActionMap Get() { return m_Wrapper.m_UiAction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UiActionActions set) { return set.Get(); }
        public void AddCallbacks(IUiActionActions instance)
        {
            if (instance == null || m_Wrapper.m_UiActionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UiActionActionsCallbackInterfaces.Add(instance);
            @Reset.started += instance.OnReset;
            @Reset.performed += instance.OnReset;
            @Reset.canceled += instance.OnReset;
            @FastForward.started += instance.OnFastForward;
            @FastForward.performed += instance.OnFastForward;
            @FastForward.canceled += instance.OnFastForward;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
        }

        private void UnregisterCallbacks(IUiActionActions instance)
        {
            @Reset.started -= instance.OnReset;
            @Reset.performed -= instance.OnReset;
            @Reset.canceled -= instance.OnReset;
            @FastForward.started -= instance.OnFastForward;
            @FastForward.performed -= instance.OnFastForward;
            @FastForward.canceled -= instance.OnFastForward;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
        }

        public void RemoveCallbacks(IUiActionActions instance)
        {
            if (m_Wrapper.m_UiActionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUiActionActions instance)
        {
            foreach (var item in m_Wrapper.m_UiActionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UiActionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UiActionActions @UiAction => new UiActionActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IUiActionActions
    {
        void OnReset(InputAction.CallbackContext context);
        void OnFastForward(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
