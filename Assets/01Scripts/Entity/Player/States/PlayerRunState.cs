using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName) : base(player, playerStateMachine, animationBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Vector3 movementDir = _player.PlayerInput.MovementDir;
        // _player.SetVelocity(movementDir.x * _player.movementData.targetRunSpeed, _player.RB.velocity.y, movementDir.z * _player.movementData.targetRunSpeed);
        _player.DirectMoveable.SetVelocity(movementDir.x, movementDir.y, movementDir.z);

        if (Mathf.Abs(movementDir.x) < Mathf.Epsilon && Mathf.Abs(movementDir.z) < Mathf.Epsilon)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
