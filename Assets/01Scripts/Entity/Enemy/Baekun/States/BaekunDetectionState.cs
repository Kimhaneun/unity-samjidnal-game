using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaekunDetectionState : EnemyState<BaekunStateEnum>
{
    public BaekunDetectionState(Enemy enemy, EnemyStateMachine<BaekunStateEnum> enemyStateMachine, string animationBoolName) : base(enemy, enemyStateMachine, animationBoolName)
    {
    }
}
