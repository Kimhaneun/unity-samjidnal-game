using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class PlayerMovement : MonoBehaviour, IMovement, IDirectMoveable
{
    #region COMPONENTS
    public Rigidbody RB { get; protected set; }
    public Collider Collider { get; protected set; }
    #endregion

    #region STATE PARAMETERS
    public bool IsFacingRight { get; protected set; }
    public bool IsJumping { get; protected set; }
    public bool IsDashing { get; protected set; }

    // Timers
    public float LastOnGroundTime { get; protected set; }

    //Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;
    #endregion

    #region CHECK PARAMETERS
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector3 _groundCheckSize;
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector3 _wallCheckSize;
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    private Collider[] _groundCollider;
    private int _groundOverlapCount = 1;
    #endregion

    [field: SerializeField] public MovementData movementData { get; protected set; }
    private Entity _entity;

    // private Vector3 _velocity;
    // public Vector3 Velocity => _velocity;

    [SerializeField] private InputHandler _inputHandler;
    public InputHandler PlayerInput => _inputHandler;

    public bool DetectGround() => IsGroundDetected();

    public bool IsJumpFalling => _isJumpFalling;

    private float _lerpAmount = 1f;

    private const float DEFAULT_GRAVITY_VALUE = 0.03f;
    private float _gravityVelocity;

    private void Awake()
    {
        _groundCollider = new Collider[_groundOverlapCount];

        RB = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }

    private void Start()
    {
        IsFacingRight = true;
    }

    private void Update()
    {
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        #endregion

        #region JUMP CHECKS
        if (IsJumping && RB.linearVelocity.y < 0)
        {
            IsJumping = false;

            _isJumpFalling = true;
        }

        if (LastOnGroundTime > Mathf.Epsilon && !IsJumping)
        {
            _isJumpCut = false;

            _isJumpFalling = false;
        }
        #endregion

        #region GRAVITY
        SetGravityScale();
        #endregion
    }

    private void FixedUpdate()
    {
        if (!IsDashing)
            Run(_lerpAmount);
    }

    public void Initialize(Entity entity)
    {
        _entity = entity;
    }

    public void SetVelocity(float x, float y, float z, bool doNotTurn = true)
    {
        RB.linearVelocity = new Vector3(x, y, z);

        if (!doNotTurn)
            CheckDirectionToFace(x);
    }

    public void StopImmediately(bool withAxisY, bool withAxisZ)
    {
        if (withAxisY && withAxisZ)
            RB.linearVelocity = Vector3.zero;
        else if (withAxisY)
            RB.linearVelocity = new Vector3(0, 0, RB.linearVelocity.z);
        else if (withAxisZ)
            RB.linearVelocity = new Vector3(0, RB.linearVelocity.y, 0);
        else
            RB.linearVelocity = new Vector3(RB.linearVelocity.x, 0, 0);
    }

    // ������ �����ε� ���߿� 
    // public void SetDestination(Vector3 destination) { }

    public void SetJump()
    {
        if (!IsDashing)
        {
            if (CanJump())
            {
                IsJumping = true;
                _isJumpCut = false;
                _isJumpFalling = false;

                Jump();
            }
        }
    }

    private bool IsGroundDetected()
    {
        if (!IsDashing && !IsJumping)
        {
            LastOnGroundTime = movementData.coyoteTime;

            int count = Physics.OverlapBoxNonAlloc(_groundCheckPoint.position, _groundCheckSize * 0.5f, _groundCollider, Quaternion.identity, _groundLayer);
            return count > 0;
        }
        return true;
    }

    private void SetGravityScale()
    {
        float gravity = movementData.gravityScale;
        if(IsGroundDetected())
        {
            _gravityVelocity = -DEFAULT_GRAVITY_VALUE;
        }
        else
        {
            if (_isJumpCut)
            {
                gravity *= movementData.jumpCutGravityMult;
            }
            else if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.linearVelocity.y) < movementData.jumpHangTimeThreshold)
            {
                gravity *= movementData.jumpHangGravityMult;
            }
            else if (RB.linearVelocity.y < 0)
            {
                gravity *= movementData.fallGravityMult;
            }
            _gravityVelocity += gravity * Time.fixedDeltaTime;
        }
        RB.linearVelocity = new Vector3(RB.linearVelocity.x, Mathf.Max(-_gravityVelocity, -movementData.maxFallSpeed), RB.linearVelocity.z);
    }

    private void Run(float lerpAmount)
    {
        Vector3 targetSpeed = new Vector3(_inputHandler.MovementDir.x, 0, _inputHandler.MovementDir.z) * movementData.runMaxSpeed;
        targetSpeed = Vector3.Lerp(new Vector3(RB.linearVelocity.x, 0, RB.linearVelocity.z), targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        if (LastOnGroundTime > 0)
            accelRate = (targetSpeed.magnitude > 0.01f) ? movementData.runAccelAmount : movementData.runDeccelAmount;
        else
            accelRate = (targetSpeed.magnitude > 0.01f) ? movementData.runAccelAmount * movementData.accelInAir : movementData.runDeccelAmount * movementData.deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.linearVelocity.y) < movementData.jumpHangTimeThreshold)
        {
            accelRate *= movementData.jumpHangAccelerationMult;
            targetSpeed *= movementData.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        if (movementData.doConserveMomentum && RB.linearVelocity.magnitude > targetSpeed.magnitude && Vector3.Dot(RB.linearVelocity, targetSpeed) > 0 && targetSpeed.magnitude > 0.01f && LastOnGroundTime < 0)
        {
            accelRate = 0;
        }
        #endregion

        Vector3 speedDif = targetSpeed - new Vector3(RB.linearVelocity.x, 0, RB.linearVelocity.z);

        Vector3 movement = speedDif * accelRate;

        if (float.IsNaN(movement.x) || float.IsNaN(movement.y) || float.IsNaN(movement.z))
        {
            movement = Vector3.zero;
        }

        RB.AddForce(movement, ForceMode.Force);
    }

    private void Jump()
    {
        LastOnGroundTime = 0;

        #region Perform Jump
        float force = movementData.jumpForce;
        if (RB.linearVelocity.y < 0)
            force -= RB.linearVelocity.y;

        RB.AddForce(Vector3.up * force, ForceMode.Impulse);
        #endregion
    }

    private void CheckDirectionToFace(float x)
    {
        if (Mathf.Abs(x) > Mathf.Epsilon)
        {
            bool isMovingRight = x > Mathf.Epsilon;
            if (isMovingRight != IsFacingRight)
                Turn();
        }
    }

    private void Turn()
    {
        float yRotation = IsFacingRight ? 180f : 0;
        transform.rotation = Quaternion.Euler(transform.rotation.x, yRotation, transform.rotation.z);
        IsFacingRight = !IsFacingRight;
    }

    private bool CanJump()
    {
        return LastOnGroundTime > Mathf.Epsilon && !IsJumping;
    }

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(_frontWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawCube(_backWallCheckPoint.position, _wallCheckSize);
    }
    #endregion
}