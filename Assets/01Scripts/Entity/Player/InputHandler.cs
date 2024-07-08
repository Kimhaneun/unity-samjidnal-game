using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputHandler")]
public class InputHandler : ScriptableObject, PlayerActions.IInputActionActions
{
    #region INPUT EVENT SETION
    public event Action JumpEvent;
    public event Action PrimaryAttackEvent;
    #endregion

    #region INPUT VALUE SECTION
    public Vector3 MovementDir { get; private set; }
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
        MovementDir = context.ReadValue<Vector3>();

        if (float.IsNaN(MovementDir.x) || float.IsNaN(MovementDir.y) || float.IsNaN(MovementDir.z))
        {
            MovementDir = Vector3.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
            JumpEvent?.Invoke();
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PrimaryAttackEvent?.Invoke();
        }
    }
}
