using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaekunAttackState : EnemyState<BaekunStateEnum>
{
    public BaekunAttackState(Enemy enemy, EnemyStateMachine<BaekunStateEnum> enemyStateMachine, string animationBoolName) : base(enemy, enemyStateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.Movement.StopImmediately(true, true);
    }

    public override void Exit()
    {
        _enemy.lastAttackTime = Time.time;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_triggerCall)
            _stateMachine.ChangeState(BaekunStateEnum.Chase);
    }
}
