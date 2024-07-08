using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
    }

    private void AnimationFinishTrigger()
    {
        _enemy.AnimationFinishTrigger();
    }

    private void CastDamage()
    {
        _enemy.Attack();
    }
}
