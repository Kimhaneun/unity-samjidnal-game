using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName) : base(player, playerStateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.Movement.SetJump();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_player.Movement.IsJumpFalling)
            _player.StateMachine.ChangeState(PlayerStateEnum.Fall);
    }
}
