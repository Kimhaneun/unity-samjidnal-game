using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputHandler")]
public class InputHandler : ScriptableObject, PlayerActions.IInputActionActions
{
    #region INPUT EVENT SETION
    // public event Action newEvent;
    #endregion

    #region INPUT VALUE SECTION
    public float InputX { get; private set; }
    public float InputY { get; private set;}
    #endregion

    private PlayerActions _playerActions;

    private void OnEnable()
    {
        if(_playerActions == null)
        {
            _playerActions = new PlayerActions();
            _playerActions.InputAction.SetCallbacks(instance: this);
        }

        _playerActions.InputAction.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        InputX = context.ReadValue<float>();
        InputY = context.ReadValue<float>();
    }
}
