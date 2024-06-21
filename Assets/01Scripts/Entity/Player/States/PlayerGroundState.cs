using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName) : base(player, playerStateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.JumpEvent += HandleJumpEvent;
        _player.PlayerInput.PrimaryAttackEvent += HandlePrimaryAttackEvent;
    }

    public override void Exit()
    {
        _player.PlayerInput.JumpEvent -= HandleJumpEvent;
        _player.PlayerInput.PrimaryAttackEvent -= HandlePrimaryAttackEvent;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_player.Movement.DetectGround())
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
    }

    #region HANDLING INPUT SECTION
    private void HandleJumpEvent()
    {
        if (_player.Movement.DetectGround())
            _stateMachine.ChangeState(PlayerStateEnum.Jump);
    }

    private void HandlePrimaryAttackEvent()
    {
        if (_player.Movement.DetectGround())
            _stateMachine.ChangeState(PlayerStateEnum.PrimaryAttack);
    }
    #endregion
}
