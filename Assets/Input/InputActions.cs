// GENERATED AUTOMATICALLY FROM 'Assets/Input/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerUI"",
            ""id"": ""673ba580-57ab-4c1b-a500-2a9a0ff82dee"",
            ""actions"": [
                {
                    ""name"": ""SubmitPotion"",
                    ""type"": ""Button"",
                    ""id"": ""04b58c9d-29b9-491f-8af4-fb4be7b5572b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Switch"",
                    ""type"": ""Button"",
                    ""id"": ""c5a0694a-b38f-4c2e-b2e4-bfc4d874d9ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reset"",
                    ""type"": ""Button"",
                    ""id"": ""59482f4c-a8cb-4ba6-9c98-626548d66f42"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3ff0a701-b75d-4ae1-9e14-a8337cee607d"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SubmitPotion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1c5f881d-5b85-470b-94f6-74d56ff75643"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bdf1a78-fd96-4d98-8914-5d0a5dc544a1"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerUI
        m_PlayerUI = asset.FindActionMap("PlayerUI", throwIfNotFound: true);
        m_PlayerUI_SubmitPotion = m_PlayerUI.FindAction("SubmitPotion", throwIfNotFound: true);
        m_PlayerUI_Switch = m_PlayerUI.FindAction("Switch", throwIfNotFound: true);
        m_PlayerUI_Reset = m_PlayerUI.FindAction("Reset", throwIfNotFound: true);
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

    // PlayerUI
    private readonly InputActionMap m_PlayerUI;
    private IPlayerUIActions m_PlayerUIActionsCallbackInterface;
    private readonly InputAction m_PlayerUI_SubmitPotion;
    private readonly InputAction m_PlayerUI_Switch;
    private readonly InputAction m_PlayerUI_Reset;
    public struct PlayerUIActions
    {
        private @InputActions m_Wrapper;
        public PlayerUIActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @SubmitPotion => m_Wrapper.m_PlayerUI_SubmitPotion;
        public InputAction @Switch => m_Wrapper.m_PlayerUI_Switch;
        public InputAction @Reset => m_Wrapper.m_PlayerUI_Reset;
        public InputActionMap Get() { return m_Wrapper.m_PlayerUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerUIActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerUIActions instance)
        {
            if (m_Wrapper.m_PlayerUIActionsCallbackInterface != null)
            {
                @SubmitPotion.started -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnSubmitPotion;
                @SubmitPotion.performed -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnSubmitPotion;
                @SubmitPotion.canceled -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnSubmitPotion;
                @Switch.started -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnSwitch;
                @Switch.performed -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnSwitch;
                @Switch.canceled -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnSwitch;
                @Reset.started -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnReset;
                @Reset.performed -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnReset;
                @Reset.canceled -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnReset;
            }
            m_Wrapper.m_PlayerUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SubmitPotion.started += instance.OnSubmitPotion;
                @SubmitPotion.performed += instance.OnSubmitPotion;
                @SubmitPotion.canceled += instance.OnSubmitPotion;
                @Switch.started += instance.OnSwitch;
                @Switch.performed += instance.OnSwitch;
                @Switch.canceled += instance.OnSwitch;
                @Reset.started += instance.OnReset;
                @Reset.performed += instance.OnReset;
                @Reset.canceled += instance.OnReset;
            }
        }
    }
    public PlayerUIActions @PlayerUI => new PlayerUIActions(this);
    public interface IPlayerUIActions
    {
        void OnSubmitPotion(InputAction.CallbackContext context);
        void OnSwitch(InputAction.CallbackContext context);
        void OnReset(InputAction.CallbackContext context);
    }
}
