using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName) : base(player, playerStateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false, true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Vector3 movementDir = _player.PlayerInput.MovementDir;

        if (Mathf.Abs(movementDir.x) > Mathf.Epsilon || Mathf.Abs(movementDir.z) > Mathf.Epsilon)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Run);
        }
    }
}
