using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaekunDeathState : EnemyState<BaekunStateEnum>
{

    private int _deadBodyLayer = LayerMask.NameToLayer("DeadBody");
    public BaekunDeathState(Enemy enemy, EnemyStateMachine<BaekunStateEnum> enemyStateMachine, string animationBoolName) : base(enemy, enemyStateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.gameObject.layer = _deadBodyLayer;
    }

    public override void UpdateState()
    {
        base.UpdateState();
       
    }
}
