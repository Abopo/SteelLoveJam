// GENERATED AUTOMATICALLY FROM 'Assets/Settings/GameInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""Racing"",
            ""id"": ""8aa31640-5e83-41d8-8dd8-ce5dff84399b"",
            ""actions"": [
                {
                    ""name"": ""MainThruster"",
                    ""type"": ""Value"",
                    ""id"": ""d261877e-1dfa-4d8d-869f-f99ef7a4939a"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RotationThrusters"",
                    ""type"": ""Value"",
                    ""id"": ""45a1d724-176c-4fca-98d8-f2c426e4d64f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftThruster"",
                    ""type"": ""Value"",
                    ""id"": ""7488f641-036c-48a4-9ff2-6c6745fa2674"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightThruster"",
                    ""type"": ""Value"",
                    ""id"": ""621808d5-3a14-40e6-8d44-41e9842d94c5"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ReverseThruster"",
                    ""type"": ""Button"",
                    ""id"": ""37ee053b-8fbc-4665-bf6c-410aedab0c10"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""a74c7fd5-a293-40ba-9a13-c04b423c3e95"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StepLeft"",
                    ""type"": ""Button"",
                    ""id"": ""541db52a-b2c7-4c14-b2f7-31ab53c532c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                },
                {
                    ""name"": ""StepRight"",
                    ""type"": ""Button"",
                    ""id"": ""cb9fb9a3-bd09-42b8-a1c7-f15795d93385"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7c08930d-5b4c-412e-a3d3-11d7c82896ff"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainThruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afd1e610-8368-43a0-a793-a8889fbef424"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainThruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8062ea84-9206-4ba1-b919-cd18dad864ca"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotationThrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard QE"",
                    ""id"": ""7cf894ed-abe0-4660-b0e2-868b129c8ba6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotationThrusters"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fc4d1197-ce61-4083-81d0-f0a9e6a9bc14"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotationThrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9f8ca311-6b1e-42b7-8854-b795ffc2fa14"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotationThrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b92b73f6-eccf-4919-9cae-d7f11d4c09c9"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotationThrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f16a7617-fb82-4805-b639-22daae52a50c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotationThrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2291f148-fe7b-46a0-a1fc-f117d2092edd"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftThruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e390258-8212-4d09-bfc3-274e5ed29607"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftThruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d2d22c6-cea0-4f6e-b6e8-97ec5ce1e171"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightThruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5ea9cc2-fbd8-4907-8538-925a297cfd5d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightThruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5286774b-3321-43d0-8467-6fbbe1a2b78e"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ReverseThruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ffa492f-347b-46ba-8dc1-c92349b05499"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ReverseThruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0cfa937d-7e14-4bc0-992c-c9dd48ad6ce8"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e48d7699-224c-4e14-b1f0-be2667c08711"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""457923c9-c9ca-4f3b-9c9a-42d28e04647f"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StepLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df578efb-f61d-4c14-acb6-72ed930cb4e0"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StepRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BreakRoom"",
            ""id"": ""787b259f-294b-4efe-a036-bb898c655bee"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""3793d084-f132-449b-b7f7-182c656ec9b8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""38948878-e5eb-4575-b9ff-f045d21824b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d3e8b684-af78-49e4-a28e-97909f3bdc29"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard "",
                    ""id"": ""483daed1-a60a-40c4-a3c1-7b7f65485bf3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9d01586a-d9bd-4eb2-bb65-1a517e26335f"",
                    ""path"": ""<Keyboard>/#(W)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1e1513b4-d47b-4570-9ed1-8c10472f8178"",
                    ""path"": ""<Keyboard>/#(S)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c2a889a3-c2ef-49c1-bf0c-7ea7c33d111b"",
                    ""path"": ""<Keyboard>/#(A)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4f3dcfcf-c3ce-4b8b-8239-872fb3308dd1"",
                    ""path"": ""<Keyboard>/#(D)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""88869ed0-03e4-4813-ab01-7ae8e4783a13"",
                    ""path"": ""<Keyboard>/#(E)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b093446-6cfa-4c72-bc17-e149c3ee6776"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""f342990e-1d30-4bb0-8c59-ab4629f408f4"",
            ""actions"": [
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""c16b2b93-1cff-4b3b-ad6d-bbb3abf89cdb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""2438b6fe-1bdb-497e-b647-9e8918fee4ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveCursor"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a5084261-34b1-4b3a-925f-dcdba3cc794f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""26e1ac97-ab70-4b82-b863-a73b97f72ba7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c11728e9-f6e5-4df1-9c49-1fb7641df9cd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53e1deb4-3d45-46f9-9687-66c0c8be2e64"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee5af364-2624-4e3a-8a75-777cc9f3325c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e19c1f7-c46c-40f4-ba4b-3f417156c6c9"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c872487-d289-4ff6-8411-816c2a6f45be"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b7df6ef-758a-41c0-a437-dcc7795e6165"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard WASD"",
                    ""id"": ""0d5ba9cc-f491-4884-80c8-b69de13a718f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fe52446f-0816-4c03-902f-0fa17cfe3c60"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8d4aa9d8-70cb-4553-b5bc-30f28cc018dc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2ba419bf-547d-4f53-95dc-d1846d48b2c9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7a63e519-e3ef-461d-996a-6605f5484ea7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Racing
        m_Racing = asset.FindActionMap("Racing", throwIfNotFound: true);
        m_Racing_MainThruster = m_Racing.FindAction("MainThruster", throwIfNotFound: true);
        m_Racing_RotationThrusters = m_Racing.FindAction("RotationThrusters", throwIfNotFound: true);
        m_Racing_LeftThruster = m_Racing.FindAction("LeftThruster", throwIfNotFound: true);
        m_Racing_RightThruster = m_Racing.FindAction("RightThruster", throwIfNotFound: true);
        m_Racing_ReverseThruster = m_Racing.FindAction("ReverseThruster", throwIfNotFound: true);
        m_Racing_Pause = m_Racing.FindAction("Pause", throwIfNotFound: true);
        m_Racing_StepLeft = m_Racing.FindAction("StepLeft", throwIfNotFound: true);
        m_Racing_StepRight = m_Racing.FindAction("StepRight", throwIfNotFound: true);
        // BreakRoom
        m_BreakRoom = asset.FindActionMap("BreakRoom", throwIfNotFound: true);
        m_BreakRoom_Movement = m_BreakRoom.FindAction("Movement", throwIfNotFound: true);
        m_BreakRoom_Interact = m_BreakRoom.FindAction("Interact", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Confirm = m_UI.FindAction("Confirm", throwIfNotFound: true);
        m_UI_Back = m_UI.FindAction("Back", throwIfNotFound: true);
        m_UI_MoveCursor = m_UI.FindAction("MoveCursor", throwIfNotFound: true);
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

    // Racing
    private readonly InputActionMap m_Racing;
    private IRacingActions m_RacingActionsCallbackInterface;
    private readonly InputAction m_Racing_MainThruster;
    private readonly InputAction m_Racing_RotationThrusters;
    private readonly InputAction m_Racing_LeftThruster;
    private readonly InputAction m_Racing_RightThruster;
    private readonly InputAction m_Racing_ReverseThruster;
    private readonly InputAction m_Racing_Pause;
    private readonly InputAction m_Racing_StepLeft;
    private readonly InputAction m_Racing_StepRight;
    public struct RacingActions
    {
        private @GameInput m_Wrapper;
        public RacingActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MainThruster => m_Wrapper.m_Racing_MainThruster;
        public InputAction @RotationThrusters => m_Wrapper.m_Racing_RotationThrusters;
        public InputAction @LeftThruster => m_Wrapper.m_Racing_LeftThruster;
        public InputAction @RightThruster => m_Wrapper.m_Racing_RightThruster;
        public InputAction @ReverseThruster => m_Wrapper.m_Racing_ReverseThruster;
        public InputAction @Pause => m_Wrapper.m_Racing_Pause;
        public InputAction @StepLeft => m_Wrapper.m_Racing_StepLeft;
        public InputAction @StepRight => m_Wrapper.m_Racing_StepRight;
        public InputActionMap Get() { return m_Wrapper.m_Racing; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RacingActions set) { return set.Get(); }
        public void SetCallbacks(IRacingActions instance)
        {
            if (m_Wrapper.m_RacingActionsCallbackInterface != null)
            {
                @MainThruster.started -= m_Wrapper.m_RacingActionsCallbackInterface.OnMainThruster;
                @MainThruster.performed -= m_Wrapper.m_RacingActionsCallbackInterface.OnMainThruster;
                @MainThruster.canceled -= m_Wrapper.m_RacingActionsCallbackInterface.OnMainThruster;
                @RotationThrusters.started -= m_Wrapper.m_RacingActionsCallbackInterface.OnRotationThrusters;
                @RotationThrusters.performed -= m_Wrapper.m_RacingActionsCallbackInterface.OnRotationThrusters;
                @RotationThrusters.canceled -= m_Wrapper.m_RacingActionsCallbackInterface.OnRotationThrusters;
                @LeftThruster.started -= m_Wrapper.m_RacingActionsCallbackInterface.OnLeftThruster;
                @LeftThruster.performed -= m_Wrapper.m_RacingActionsCallbackInterface.OnLeftThruster;
                @LeftThruster.canceled -= m_Wrapper.m_RacingActionsCallbackInterface.OnLeftThruster;
                @RightThruster.started -= m_Wrapper.m_RacingActionsCallbackInterface.OnRightThruster;
                @RightThruster.performed -= m_Wrapper.m_RacingActionsCallbackInterface.OnRightThruster;
                @RightThruster.canceled -= m_Wrapper.m_RacingActionsCallbackInterface.OnRightThruster;
                @ReverseThruster.started -= m_Wrapper.m_RacingActionsCallbackInterface.OnReverseThruster;
                @ReverseThruster.performed -= m_Wrapper.m_RacingActionsCallbackInterface.OnReverseThruster;
                @ReverseThruster.canceled -= m_Wrapper.m_RacingActionsCallbackInterface.OnReverseThruster;
                @Pause.started -= m_Wrapper.m_RacingActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_RacingActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_RacingActionsCallbackInterface.OnPause;
                @StepLeft.started -= m_Wrapper.m_RacingActionsCallbackInterface.OnStepLeft;
                @StepLeft.performed -= m_Wrapper.m_RacingActionsCallbackInterface.OnStepLeft;
                @StepLeft.canceled -= m_Wrapper.m_RacingActionsCallbackInterface.OnStepLeft;
                @StepRight.started -= m_Wrapper.m_RacingActionsCallbackInterface.OnStepRight;
                @StepRight.performed -= m_Wrapper.m_RacingActionsCallbackInterface.OnStepRight;
                @StepRight.canceled -= m_Wrapper.m_RacingActionsCallbackInterface.OnStepRight;
            }
            m_Wrapper.m_RacingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MainThruster.started += instance.OnMainThruster;
                @MainThruster.performed += instance.OnMainThruster;
                @MainThruster.canceled += instance.OnMainThruster;
                @RotationThrusters.started += instance.OnRotationThrusters;
                @RotationThrusters.performed += instance.OnRotationThrusters;
                @RotationThrusters.canceled += instance.OnRotationThrusters;
                @LeftThruster.started += instance.OnLeftThruster;
                @LeftThruster.performed += instance.OnLeftThruster;
                @LeftThruster.canceled += instance.OnLeftThruster;
                @RightThruster.started += instance.OnRightThruster;
                @RightThruster.performed += instance.OnRightThruster;
                @RightThruster.canceled += instance.OnRightThruster;
                @ReverseThruster.started += instance.OnReverseThruster;
                @ReverseThruster.performed += instance.OnReverseThruster;
                @ReverseThruster.canceled += instance.OnReverseThruster;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @StepLeft.started += instance.OnStepLeft;
                @StepLeft.performed += instance.OnStepLeft;
                @StepLeft.canceled += instance.OnStepLeft;
                @StepRight.started += instance.OnStepRight;
                @StepRight.performed += instance.OnStepRight;
                @StepRight.canceled += instance.OnStepRight;
            }
        }
    }
    public RacingActions @Racing => new RacingActions(this);

    // BreakRoom
    private readonly InputActionMap m_BreakRoom;
    private IBreakRoomActions m_BreakRoomActionsCallbackInterface;
    private readonly InputAction m_BreakRoom_Movement;
    private readonly InputAction m_BreakRoom_Interact;
    public struct BreakRoomActions
    {
        private @GameInput m_Wrapper;
        public BreakRoomActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_BreakRoom_Movement;
        public InputAction @Interact => m_Wrapper.m_BreakRoom_Interact;
        public InputActionMap Get() { return m_Wrapper.m_BreakRoom; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BreakRoomActions set) { return set.Get(); }
        public void SetCallbacks(IBreakRoomActions instance)
        {
            if (m_Wrapper.m_BreakRoomActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_BreakRoomActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_BreakRoomActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_BreakRoomActionsCallbackInterface.OnMovement;
                @Interact.started -= m_Wrapper.m_BreakRoomActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_BreakRoomActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_BreakRoomActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_BreakRoomActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public BreakRoomActions @BreakRoom => new BreakRoomActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Confirm;
    private readonly InputAction m_UI_Back;
    private readonly InputAction m_UI_MoveCursor;
    public struct UIActions
    {
        private @GameInput m_Wrapper;
        public UIActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Confirm => m_Wrapper.m_UI_Confirm;
        public InputAction @Back => m_Wrapper.m_UI_Back;
        public InputAction @MoveCursor => m_Wrapper.m_UI_MoveCursor;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Confirm.started -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                @Confirm.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                @Confirm.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                @Back.started -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @MoveCursor.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMoveCursor;
                @MoveCursor.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMoveCursor;
                @MoveCursor.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMoveCursor;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Confirm.started += instance.OnConfirm;
                @Confirm.performed += instance.OnConfirm;
                @Confirm.canceled += instance.OnConfirm;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
                @MoveCursor.started += instance.OnMoveCursor;
                @MoveCursor.performed += instance.OnMoveCursor;
                @MoveCursor.canceled += instance.OnMoveCursor;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IRacingActions
    {
        void OnMainThruster(InputAction.CallbackContext context);
        void OnRotationThrusters(InputAction.CallbackContext context);
        void OnLeftThruster(InputAction.CallbackContext context);
        void OnRightThruster(InputAction.CallbackContext context);
        void OnReverseThruster(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnStepLeft(InputAction.CallbackContext context);
        void OnStepRight(InputAction.CallbackContext context);
    }
    public interface IBreakRoomActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnConfirm(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnMoveCursor(InputAction.CallbackContext context);
    }
}