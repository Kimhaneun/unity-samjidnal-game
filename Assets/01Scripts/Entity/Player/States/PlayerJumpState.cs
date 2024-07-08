using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName) : base(player, playerStateMachine, animationBoolName)
    {
        // 점프는 한 번만?
        // 일단 한 번으로 만들어 보고 아님 이 단으로 만들자
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
