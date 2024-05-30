using System;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;
using static UnityEditor.Experimental.GraphView.GraphView;
using Debug = UnityEngine.Debug;

public class Player : Entity
{
    // �����ӿ� �ʿ��� �������� ���������� ���� SO�� ���� ���� �����ðž�.
    // ���� ��������!

    // SO�� �����ϴ� SO �Ǵ� Class�� ���� 
    public MovementData movementData;

    // �������� �� �ʿ��ϸ� �� �� ����ϰ�...
    private float _defaultRunSpeed;
    private float _defaultJumpForce;
    private float _defaultDashSpeed;

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

    //protected void Start()
    //{
    //    StateMachine.Initialize(PlayerStateEnum.Idle, player: this);

    //    _defaultRunSpeed = movementData.targetRunSpeed;
    //    _defaultJumpForce = movementData.jumpForce;
    //    _defaultDashSpeed = movementData.dashSpeed;
    //}

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(PlayerStateEnum.Idle, player: this);

        _defaultRunSpeed = movementData.targetRunSpeed;
        _defaultJumpForce = movementData.jumpForce;
        _defaultDashSpeed = movementData.dashSpeed;
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public override void Attack()
    {

    }
}
