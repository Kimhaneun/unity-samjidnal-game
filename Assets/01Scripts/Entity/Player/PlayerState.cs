using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;

    protected int _animationBoolHash;
    protected readonly int _yVelocityHash = Animator.StringToHash(name: "yVelocity");
    protected bool _triggerCall;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName)
    {
        _player = player;
        _stateMachine = playerStateMachine;
        _animationBoolHash = Animator.StringToHash(animationBoolName);
    }

    // State Enter
    public virtual void Enter()
    {
        _triggerCall = false;
        _player.Animator.SetBool(_animationBoolHash, value: true);
    }

    // State Exit
    public virtual void Exit()
    {
        _player.Animator.SetBool(_animationBoolHash, value: false);
    }

    public virtual void UpdateState()
    {
    }

    public virtual void AnimationFinishTrigger()
    {
        _triggerCall = true;
    }
}
