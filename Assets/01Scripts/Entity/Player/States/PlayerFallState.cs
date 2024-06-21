using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName) : base(player, playerStateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_player.Movement.DetectGround())
            _player.StateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}
