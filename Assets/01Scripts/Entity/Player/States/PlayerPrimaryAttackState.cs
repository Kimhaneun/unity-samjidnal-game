using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName) : base(player, playerStateMachine, animationBoolName)
    {
    }

    private int _comboCounter;
    private float _lastAttackTime;
    private float _comboWindow = 0.8f;

    private Coroutine _delayCoroutine;

    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    public override void Enter()
    {
        base.Enter();
        if (_comboCounter > 1 || Time.time >= _lastAttackTime + _comboWindow)
        {
            _comboCounter = 0;
        }

        _player.Animator.SetInteger(_comboCounterHash, _comboCounter);
        _player.attackData.currentComboCounter = _comboCounter;
        _player.Animator.speed = _player.attackData.attackSpeed;

        Vector3 movementDir = _player.PlayerInput.MovementDir;
        Vector3 attackMovementPower = _player.attackData.attackMovement[_comboCounter];

        _player.DirectMoveable.SetVelocity(movementDir.x, movementDir.y, movementDir.z, false); // 방금 수정했어

        // 수정해줘
        // _player.SetVelocity(movementDir.x * Mathf.Abs(attackMovementPower.x), movementDir.y, movementDir.z * Mathf.Abs(attackMovementPower.z));

        float delayTime = 0.2f;
        _delayCoroutine = _player.StartDelayCallback(delayTime, () => _player.StopImmediately(false, false));
    }

    public override void Exit()
    {
        ++_comboCounter;
        _lastAttackTime = Time.time;
        _player.Animator.speed = 1f;

        _player.StopCoroutine(_delayCoroutine);
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_triggerCall)
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}
