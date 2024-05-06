using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;
    protected Rigidbody _rb;

    protected int _animationBoolHash;
    protected readonly int _yVelocityHash = Animator.StringToHash(name: "yVelocity");
    protected bool _triggerCall;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, string animationBoolName)
    {
        _player = player;
        _stateMachine = playerStateMachine;
        _animationBoolHash = Animator.StringToHash(animationBoolName);
        _rb = _player.RB;
    }

    // State Enter
    public virtual void Enter()
    {
        _player.Animator.SetBool(_animationBoolHash, value: false);
    }

    // State Exit
    public virtual void Exit()
    {
        _player.Animator.SetBool(_animationBoolHash, value: false);
    }

    // �� ������ �� ���� �Լ��� ��°������ �߰����� �ʾҾ��� 
    public virtual void UpdateState()
    {
        _player.Animator.SetFloat(_yVelocityHash, _rb.velocity.y);
    }

    public virtual void AnimationFinishTrigger()
    {
        _triggerCall = true;
    }
}
