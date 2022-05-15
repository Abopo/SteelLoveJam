using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//[CreateAssetMenu(fileName = "InputRreader", menuName = "Game/Input Reader")] // commented out because we only need one
public class InputReader : DescriptionBaseSO, GameInput.IRacingActions, GameInput.IBreakRoomActions, GameInput.IUIActions
{
    private GameInput _gameInput;

    // Racing
    public event UnityAction<float> MainThrusterEvent = delegate { };
    public event UnityAction<float> ReverseThrusterEvent = delegate { };
    public event UnityAction<float> LeftThrustEvent = delegate { };
    public event UnityAction<float> RightThrustEvent = delegate { };
    public event UnityAction<Vector2> RotationThrustersEvent = delegate { };
    public event UnityAction StepLeftEvent = delegate { };
    public event UnityAction StepRightEvent = delegate { };
    public event UnityAction<float> BoostEvent = delegate { };
    public event UnityAction QuickTurnEvent = delegate { };
    public event UnityAction<float> BrakeEvent = delegate { };
    public event UnityAction PauseEvent = delegate { };

    // Break room
    public event UnityAction<Vector2> MovementEvent = delegate { };
    public event UnityAction<float> InteractEvent = delegate { };

    // UI
    public event UnityAction<Vector2> MoveCursorEvent = delegate { };
    public event UnityAction BackEvent = delegate { };
    public event UnityAction ConfirmEvent = delegate { };

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            _gameInput.Racing.SetCallbacks(this);
            _gameInput.BreakRoom.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

#region RacingInput
    public void OnMainThruster(InputAction.CallbackContext context)
    {
        MainThrusterEvent.Invoke(context.ReadValue<float>());
    }

    public void OnReverseThruster(InputAction.CallbackContext context)
    {
        ReverseThrusterEvent.Invoke(context.ReadValue<float>());
    }

    public void OnLeftThruster(InputAction.CallbackContext context)
    {
        LeftThrustEvent.Invoke(context.ReadValue<float>());
    }

    public void OnRightThruster(InputAction.CallbackContext context)
    {
        RightThrustEvent.Invoke(context.ReadValue<float>());
    }

    public void OnRotationThrusters(InputAction.CallbackContext context)
    {
        RotationThrustersEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnStepLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            StepLeftEvent.Invoke();
    }

    public void OnStepRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            StepRightEvent.Invoke();
    }

    public void OnBoost(InputAction.CallbackContext context) {
        BoostEvent.Invoke(context.ReadValue<float>());
    }

    public void OnQuickTurn(InputAction.CallbackContext context) {
        QuickTurnEvent.Invoke();
    }

    public void OnBrake(InputAction.CallbackContext context) {
        BrakeEvent.Invoke(context.ReadValue<float>());
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            PauseEvent.Invoke();
    }
    #endregion

#region BreakRoomInput

    public void OnMovement(InputAction.CallbackContext context) {
        MovementEvent.Invoke(context.ReadValue<Vector2>());
    }
    public void OnInteract(InputAction.CallbackContext context) {
        if (context.performed) {
            InteractEvent.Invoke(context.ReadValue<float>());
        }
    }

#endregion

#region UIInput
    public void OnMoveCursor(InputAction.CallbackContext context)
    {
        MoveCursorEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            BackEvent.Invoke();
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            ConfirmEvent.Invoke();
    }

    public void OnLeftClick(InputAction.CallbackContext context) {

    }
#endregion

    public void EnableAllInput()
    {
        _gameInput.Racing.Enable();
        _gameInput.BreakRoom.Enable();
        _gameInput.UI.Enable();
    }

    public void DisableAllInput()
    {
        _gameInput.Racing.Disable();
        _gameInput.BreakRoom.Disable();
        _gameInput.UI.Disable();
    }

    public void EnableRacingInput()
    {
        _gameInput.Racing.Enable();
    }

    public void EnableBreakRoomInput() 
    {
        _gameInput.BreakRoom.Enable();
    }

    public void EnableUIInput()
    {
        _gameInput.UI.Enable();
    }
}
