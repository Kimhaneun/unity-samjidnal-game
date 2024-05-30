using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerState
{
    public PlayerMovementState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName) : base(player, playerStateMachine, animationBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Vector3 movementDir = _player.PlayerInput.MovementDir;

        _player.SetVelocity(movementDir.x * _player.movementData.targetRunSpeed, _rb.velocity.y, movementDir.z * _player.movementData.targetRunSpeed);

        if (Mathf.Abs(movementDir.x) < Mathf.Epsilon && Mathf.Abs(movementDir.z) < Mathf.Epsilon)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
