using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public enum PlayerStateEnum
{
    Idle,
    Run,
    Jump,
    Fall,
    PrimaryAttack,
    Ground
}

public class Player : Entity
{
    [field: SerializeField] public AttackData attackData { get; protected set; }

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
    }

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
}
