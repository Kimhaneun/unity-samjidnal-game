using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum BaekunStateEnum
{
    Waiting,
    Detection,
    Chase,
    Attack,
    Death
}

public class BaekunEnemy : Enemy
{
    public EnemyStateMachine<BaekunStateEnum> stateMachine { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine<BaekunStateEnum>();

        foreach(BaekunStateEnum baekunStateEnum in Enum.GetValues(typeof(BaekunStateEnum)))
        {
            string typeName = baekunStateEnum.ToString();
            Type type = Type.GetType($"Baekun{typeName}State");

            try
            {
                EnemyState<BaekunStateEnum> state = Activator.CreateInstance(type, this, stateMachine, typeName) as EnemyState<BaekunStateEnum>;
                stateMachine.AddState(baekunStateEnum, state);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Enemy Baekun : no State found [ {typeName} ] - {ex.Message}");
            }
        }
    }

    protected override void Start()
    {
        stateMachine.Initialize(BaekunStateEnum.Waiting, this);
    }

    protected void Update()
    {
        stateMachine.CurrentState.UpdateState();
    }

    public override void Attack()
    {
        bool result = DamageCaster.IsCastDamage();
    }

    public override void AnimationFinishTrigger()
    {
        stateMachine.CurrentState.AnimationFinishTrigger();
    }

    public override void Death()
    {
        stateMachine.ChangeState(BaekunStateEnum.Death, forceMode: true);
        IsDead = true;
        CanStateChangeable = false;
    }
}
