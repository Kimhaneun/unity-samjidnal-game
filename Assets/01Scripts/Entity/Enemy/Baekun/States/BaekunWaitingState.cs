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
            // 원래는 발견 상태로 넘어가야 하는데 지금은 잠시 생략한다.
            _stateMachine.ChangeState(BaekunStateEnum.Chase);
        }
    }
}
