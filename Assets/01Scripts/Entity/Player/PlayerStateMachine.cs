using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    public Dictionary<PlayerStateEnum, PlayerState> stateDictionary;

    private Player _player;

    public PlayerStateMachine()
    {
        stateDictionary = new Dictionary<PlayerStateEnum, PlayerState>();
    }

    public void Initialize(PlayerStateEnum startState, Player player)
    {
        _player = player;
        CurrentState = stateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(PlayerStateEnum newState)
    {
        if (!_player.CanStateChangeable)
            return;

        CurrentState.Exit();
        CurrentState = stateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(PlayerStateEnum playerStateEnum, PlayerState state)
    {
        stateDictionary.Add(playerStateEnum, state);
    }
}
