using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState<T> where T : Enum
{
    protected EnemyStateMachine<T> _stateMachine;
    protected Enemy _enemy;

    protected int _animationBoolHash;
    protected bool _triggerCall;

    public EnemyState(Enemy enemy, EnemyStateMachine<T> enemyStateMachine, string animationBoolName)
    {
        _enemy = enemy;
        _stateMachine = enemyStateMachine;
        _animationBoolHash = Animator.StringToHash(animationBoolName);
    }

    // State Enter
    public virtual void Enter()
    {
        _triggerCall = false;
        _enemy.Animator.SetBool(_animationBoolHash, value: true);
    }

    // State Exit
    public virtual void Exit()
    {
        _enemy.Animator.SetBool(_animationBoolHash, value: false);
    }

    public virtual void UpdateState() { }

    public virtual void AnimationFinishTrigger()
    {
        _triggerCall = true;
    }
}
