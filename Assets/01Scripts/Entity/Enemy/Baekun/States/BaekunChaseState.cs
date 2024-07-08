using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaekunChaseState : EnemyState<BaekunStateEnum>
{
    private EnemyMovement _enemyMovement;
    public BaekunChaseState(Enemy enemy, EnemyStateMachine<BaekunStateEnum> enemyStateMachine, string animationBoolName) : base(enemy, enemyStateMachine, animationBoolName)
    {
        _enemyMovement = _enemy.Movement as EnemyMovement;
    }

    private Vector3 _targetDestination;
    private const float CHASE_UPDATE_DISTANCE = 1;

    public override void Enter()
    {
        base.Enter();
        SetDestination(_enemy.targetTransform.position);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_enemyMovement.NavMeshAgent.enabled)
            _targetDestination = _enemyMovement.NavMeshAgent.destination;

        float distance = (_targetDestination - _enemy.targetTransform.position).magnitude;
        float test = Mathf.Abs(Mathf.Sin(_targetDestination.x)) - _enemy.targetTransform.position.x;
        if (test > CHASE_UPDATE_DISTANCE)
            SetDestination(_enemy.targetTransform.position);

        Collider playerCollider = _enemy.Test();
        if (playerCollider != null)
        {
            _stateMachine.ChangeState(BaekunStateEnum.Attack);
        }
    }

    private void SetDestination(Vector3 position)
    {
        _targetDestination = position;
        _enemy.Movement.SetDestination(position);
    }
}
