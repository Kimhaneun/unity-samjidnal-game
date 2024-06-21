using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine<T> where T : Enum
{
    public EnemyState<T> CurrentState { get; private set; }
    public Dictionary<T, EnemyState<T>> StateDictionary = new Dictionary<T, EnemyState<T>>();
    public Enemy enemyBase;

    public void Initialize(T startState, Enemy enemy)
    {
        this.enemyBase = enemy;
        CurrentState = StateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(T newState, bool forceMode = false)
    {
        if (enemyBase.CanStateChangeable == false && forceMode == false)
            return;
        if (enemyBase.IsDead)
            return;

        CurrentState.Exit();
        CurrentState = StateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(T enemyStateEnum, EnemyState<T> state)
    {
        StateDictionary.Add(enemyStateEnum, state);
    }
}
