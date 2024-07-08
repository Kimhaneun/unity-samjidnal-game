using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : Entity
{
    [field: SerializeField] public MovementData movementData { get; protected set; }
    [field: SerializeField] public AttackData attackData { get; protected set; }

    [HideInInspector] public Transform targetTransform;
    [HideInInspector] public float lastAttackTime;

    private float _defaultRunSpeed;
    private float _defaultJumpForce;
    private float _defaultDashSpeed;

    [SerializeField] private Transform _chaseDistanceCheckPoint;
    [SerializeField] public float chaseDistance;
    [SerializeField] private Transform _attackDistanceCheckPoint;
    [SerializeField] public Vector3 attackDistance;
    [SerializeField] private float attackDelayTime;

    private Vector3[] _attackMovement;

    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected LayerMask _obstacleLayer;

    protected Collider[] _enemyCheckCollider;

    private int _enemyCount = 1;

    protected Collider[] _testCollider;

    protected override void Awake()
    {
        base.Awake();
        _enemyCheckCollider = new Collider[_enemyCount];

        _testCollider = new Collider[_enemyCount];
    }

    protected override void Start()
    {
        base.Start();
        _defaultRunSpeed = movementData.runMaxSpeed;
        _defaultJumpForce = movementData.jumpForce;
        _defaultDashSpeed = movementData.dashSpeed;

        _attackMovement = attackData.attackMovement;
    }

    public virtual Collider IsPlayerDetected()
    {
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, chaseDistance, _enemyCheckCollider, _playerLayer);
        return cnt >= 1 ? _enemyCheckCollider[0] : null;
    }

    public virtual bool IsObstacleInLine(float distance, Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, distance, _obstacleLayer);
    }

    public virtual Collider Test()
    {
        int cnt = Physics.OverlapBoxNonAlloc(_attackDistanceCheckPoint.position, attackDistance * 0.5f, _testCollider, quaternion.identity, _playerLayer);
        return cnt >= 1 ? _enemyCheckCollider[0] : null;
    }

    public abstract void AnimationFinishTrigger();

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_chaseDistanceCheckPoint.position, chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackDistanceCheckPoint.position, attackDistance);
        Gizmos.color = Color.white;
    }
}
