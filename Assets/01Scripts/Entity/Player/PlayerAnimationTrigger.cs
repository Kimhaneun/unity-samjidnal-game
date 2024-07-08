using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = transform.parent.GetComponent<Player>();
    }

    private void AnimationFinishTrigger()
    {
        _player.StateMachine.CurrentState.AnimationFinishTrigger();
    }

    private void CastDamage()
    {
        _player.Attack();
    }
}
