//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/DialogueSystem/Runtime/Input/DialogueInput.inputactions
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

public partial class @DialogueInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @DialogueInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DialogueInput"",
    ""maps"": [
        {
            ""name"": ""DialogueControls"",
            ""id"": ""52e64428-92f0-4245-ab76-40bc0f7fe2d2"",
            ""actions"": [
                {
                    ""name"": ""Interact"",
                    ""type"": ""Value"",
                    ""id"": ""2e7735bd-36d7-4d6a-bae0-f5f8d36df844"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""NextDialogue"",
                    ""type"": ""Value"",
                    ""id"": ""4350769d-fb47-465a-bc47-9e9e9374f424"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""956d4c59-8e08-4873-a3ea-d916d7d6bd29"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7921ece9-cd1e-482a-95bc-c76da64d1f4a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // DialogueControls
        m_DialogueControls = asset.FindActionMap("DialogueControls", throwIfNotFound: true);
        m_DialogueControls_Interact = m_DialogueControls.FindAction("Interact", throwIfNotFound: true);
        m_DialogueControls_NextDialogue = m_DialogueControls.FindAction("NextDialogue", throwIfNotFound: true);
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

    // DialogueControls
    private readonly InputActionMap m_DialogueControls;
    private List<IDialogueControlsActions> m_DialogueControlsActionsCallbackInterfaces = new List<IDialogueControlsActions>();
    private readonly InputAction m_DialogueControls_Interact;
    private readonly InputAction m_DialogueControls_NextDialogue;
    public struct DialogueControlsActions
    {
        private @DialogueInput m_Wrapper;
        public DialogueControlsActions(@DialogueInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Interact => m_Wrapper.m_DialogueControls_Interact;
        public InputAction @NextDialogue => m_Wrapper.m_DialogueControls_NextDialogue;
        public InputActionMap Get() { return m_Wrapper.m_DialogueControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DialogueControlsActions set) { return set.Get(); }
        public void AddCallbacks(IDialogueControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_DialogueControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DialogueControlsActionsCallbackInterfaces.Add(instance);
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @NextDialogue.started += instance.OnNextDialogue;
            @NextDialogue.performed += instance.OnNextDialogue;
            @NextDialogue.canceled += instance.OnNextDialogue;
        }

        private void UnregisterCallbacks(IDialogueControlsActions instance)
        {
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @NextDialogue.started -= instance.OnNextDialogue;
            @NextDialogue.performed -= instance.OnNextDialogue;
            @NextDialogue.canceled -= instance.OnNextDialogue;
        }

        public void RemoveCallbacks(IDialogueControlsActions instance)
        {
            if (m_Wrapper.m_DialogueControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDialogueControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_DialogueControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DialogueControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DialogueControlsActions @DialogueControls => new DialogueControlsActions(this);
    public interface IDialogueControlsActions
    {
        void OnInteract(InputAction.CallbackContext context);
        void OnNextDialogue(InputAction.CallbackContext context);
    }
}
