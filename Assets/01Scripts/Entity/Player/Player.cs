using System;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;
using static UnityEditor.Experimental.GraphView.GraphView;
using Debug = UnityEngine.Debug;

public enum PlayerStateEnum
{
    Idle,
    Run,
    Jump,
    Fall,
    PrimaryAttack,
    Ground
    // Dash
}

public class Player : Entity
{
    // SO를 관리하는 SO 또는 Class를 만들어서 
    // [field: SerializeField] public MovementData movementData { get; protected set; }
    [field: SerializeField] public AttackData attackData { get; protected set; }

    // 나머지는 더 필요하면 그 때 사용하고...
    //private float _defaultRunSpeed;
    //private float _defaultJumpForce;
    //private float _defaultDashSpeed;

    private Vector3[] _attackMovement;

    public PlayerStateMachine StateMachine { get; private set; }
    [SerializeField] private InputHandler _inputHandler;
    public InputHandler PlayerInput => _inputHandler;

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();

        foreach (PlayerStateEnum playerStateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = playerStateEnum.ToString();
            try
            {
                // Be careful with class names.
                Type type = Type.GetType($"Player{typeName}State");
                PlayerState state = Activator.CreateInstance(type, this, StateMachine, typeName) as PlayerState;

                StateMachine.AddState(playerStateEnum, state);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{typeName} is loading error.");
            }
        }
    }

    protected override void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, player: this);

        //_defaultRunSpeed = movementData.runMaxSpeed;
        //_defaultJumpForce = movementData.jumpForce;
        //_defaultDashSpeed = movementData.dashSpeed;
    }

    //protected override void Start()
    //{
    //    base.Start();
    //    StateMachine.Initialize(PlayerStateEnum.Idle, player: this);

    //    _defaultRunSpeed = movementData.targetRunSpeed;
    //    _defaultJumpForce = movementData.jumpForce;
    //    _defaultDashSpeed = movementData.dashSpeed;

    //    _attackMovement = attackData.attackMovement;
    //}

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public override void Attack() 
    {
        bool result = DamageCaster.IsCastDamage();
    }

    public override void Death()
    {
    }

    // public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
}
