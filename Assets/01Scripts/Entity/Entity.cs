using System;
using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region COMPONENTS
    // public Rigidbody RB { get; protected set; } // 이건 이제 필요 없어
    public Animator Animator { get; protected set; }
    public SpriteRenderer SpriteRenderer { get; protected set; }
    public DamageCaster DamageCaster { get; protected set; }
    public Health Health { get; protected set; }
    // public Collider Collider { get; protected set; }

    public IMovement Movement { get; protected set; }
    #endregion

    // public int FacingDiraction { get; protected set; }
    // -- 
    #region STATE PARAMETERS
    // public bool IsFacingRight { get; protected set; }
    // public bool IsJumping { get;  set; } // --- 더 좋은 구조는 없을까?
    // public bool IsDashing { get; protected set; }

    // Timers
    // public float LastOnGroundTime { get; protected set; }
    #endregion
    // -- 
    public bool CanStateChangeable { get; protected set; } = true;
    public bool IsDead { get; protected set; } // 


    #region CHECK PARAMETERS
    // [Header("Checks")]
    // [SerializeField] private Transform _groundCheckPoint;
    // [SerializeField] private Vector3 _groundCheckSize;
    // [Space(5)]
    // [SerializeField] private Transform _frontWallCheckPoint;
    // [SerializeField] private Transform _backWallCheckPoint;
    // [SerializeField] private Vector3 _wallCheckSize;
    #endregion

    #region LAYERS & TAGS
    // [Header("Layers & Tags")]
    // [SerializeField] private LayerMask _groundLayer;
    // private Collider[] _groundCollider;
    // private int _groundOverlapCount = 1;
    #endregion

    [SerializeField] protected EntityStat _entityStat;
    public EntityStat EntityStat => _entityStat;

    protected virtual void Awake()
    {
        // _groundCollider = new Collider[_groundOverlapCount];

        // RB = GetComponent<Rigidbody>();
        // Collider = GetComponent<Collider>();
        Movement = GetComponent<IMovement>();

        Movement.Initialize(this);

        Transform damageCasterTransform = transform.Find("DamageCaster");
        if(damageCasterTransform != null)
        {
            DamageCaster = damageCasterTransform.GetComponent<DamageCaster>();
            DamageCaster.Initialize(this);
        }

        Transform visualTransform = transform.Find("Visual");
        Animator = visualTransform.GetComponent<Animator>();
        SpriteRenderer = visualTransform.GetComponent<SpriteRenderer>();

        _entityStat = Instantiate(_entityStat);
        _entityStat.SetEntity(this);

        Health = GetComponent<Health>();
        Health?.Initialize(this);
    }

    // ---
    protected virtual void Start()
    {
        // IsFacingRight = true;
    }
    // --- 

    // Attack, Die, Delay 등을 구현 하기도 하는데 아직은 구현하지 말자
    #region DELAY CALLBACK COROUTINE
    public Coroutine StartDelayCallback(float delayTime, Action Callback)
    {
        return StartCoroutine(DelayCoroutine(delayTime, Callback));
    }
    #endregion

    #region VELOCITY CONTROL
    // public void SetVelocity(float x, float y, float z, bool doNotTurn = false)
    // {
    // RB.velocity = new Vector3(x, y, z);
    // 
    // // ---
    // if (!doNotTurn)
    // {
    //     CheckDirectionToFace(x);
    // }
    // // ---
    // }

    public void StopImmediately(bool withAxisY, bool withAxisZ)
    {
        // if (withAxisY && withAxisZ)
        //     RB.velocity = Vector3.zero;
        // else if (withAxisY)
        //     RB.velocity = new Vector3(0, 0, RB.velocity.z);
        // else if (withAxisZ)
        //     RB.velocity = new Vector3(0, RB.velocity.y, 0);
        // else
        //     RB.velocity = new Vector3(RB.velocity.x, 0, 0);
    }
    #endregion

    protected IEnumerator DelayCoroutine(float delayTime, Action callback)
    {
        yield return new WaitForSeconds(delayTime);
        callback?.Invoke();
    }

    // --- 리전으로 정리 하기 
    public virtual void CheckDirectionToFace(float x)
    {
        // if (Mathf.Abs(x) > Mathf.Epsilon)
        // {
        //     bool isMovingRight = x > Mathf.Epsilon;
        //     if (isMovingRight != IsFacingRight)
        //         Turn();
        // }
    }

    public virtual void Turn()
    {
        // float yRotation = IsFacingRight ? 180f : 0;
        // transform.rotation = Quaternion.Euler(transform.rotation.x, yRotation, transform.rotation.z);
        // IsFacingRight = !IsFacingRight;
    }
    // ---

    // 리전으로 정리 하기 
    // public virtual bool IsGroundDetected()
    // {
    // if (!IsDashing && !IsJumping)
    // {
    //     int count = Physics.OverlapBoxNonAlloc(_groundCheckPoint.position, _groundCheckSize * 0.5f, _groundCollider, Quaternion.identity, _groundLayer);
    //     return count > 0;
    // }
    // return true;
    // }

    public abstract void Attack();

    public abstract void Death();

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        // Gizmos.color = Color.green;
        // Gizmos.DrawCube(_groundCheckPoint.position, _groundCheckSize);
        // Gizmos.color = Color.blue;
        // Gizmos.DrawCube(_frontWallCheckPoint.position, _wallCheckSize);
        // Gizmos.DrawCube(_backWallCheckPoint.position, _wallCheckSize);
    }
    #endregion
}
