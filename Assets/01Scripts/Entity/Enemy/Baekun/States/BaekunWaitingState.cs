using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaekunWaitingState : EnemyState<BaekunStateEnum>
{
    public BaekunWaitingState(Enemy enemy, EnemyStateMachine<BaekunStateEnum> enemyStateMachine, string animationBoolName) : base(enemy, enemyStateMachine, animationBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Collider target = _enemy.IsPlayerDetected();

        if (target == null)
            return;

        Vector3 direction = target.transform.position - _enemy.transform.position;
        direction.y = 0;

        if (_enemy.IsObstacleInLine(direction.magnitude, direction.normalized) == false)
        {
            _enemy.targetTransform = target.transform;
            _stateMachine.ChangeState(BaekunStateEnum.Chase);
        }
    }
}
