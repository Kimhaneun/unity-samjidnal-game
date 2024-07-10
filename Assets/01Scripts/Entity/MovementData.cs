using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Entity/MovementData")]
public class MovementData : ScriptableObject
{
    // 이후 클래스를 통해 상속 받게 해 관리?
    // 아니면 또 다른 SO를 통해 관리?

    // 커스텀 윈도우 통해 SO를 관리할 필요 있을까? 고민 해 보자.

    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; 
    [HideInInspector] public float gravityScale; 

    [Space(5)]
    public float fallGravityMult;
    public float maxFallSpeed; 

    [Space(20)]

    [Header("Run")]
    // public float targetRunSpeed;

    public float runMaxSpeed;
    public float runAcceleration;
    [HideInInspector] public float runAccelAmount;
    public float runDecceleration;
    [HideInInspector] public float runDeccelAmount;
    [Space(5)]
    [Range(0f, 1)] public float accelInAir;
    [Range(0f, 1)] public float deccelInAir;
    [Space(5)]
    public bool doConserveMomentum = true;

    [Space(20)]

    [Header("Jump")]
    public float jumpHeight; 
    public float jumpTimeToApex; 
    [HideInInspector] public float jumpForce; 

    [Header("Both Jumps")]
    public float jumpCutGravityMult; 
    [Range(0f, 1)] public float jumpHangGravityMult; 
    public float jumpHangTimeThreshold; 
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce; 
    [Space(5)]
    [Range(0f, 1f)] public float wallJumpRunLerp; 
    [Range(0f, 1.5f)] public float wallJumpTime;
    public bool doTurnOnWallJump; 

    [Space(20)]

    [Header("Slide")]
    public float slideSpeed;
    public float slideAccel;

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime; 
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; 

    [Space(20)]

    [Header("Dash")]
    public int dashAmount;
    public float dashSpeed;
    public float dashSleepTime; 
    [Space(5)]
    public float dashAttackTime;
    [Space(5)]
    public float dashEndTime; 
    public Vector2 dashEndSpeed;
    [Range(0f, 1f)] public float dashEndRunLerp; 
    [Space(5)]
    public float dashRefillTime;
    [Space(5)]
    [Range(0.01f, 0.5f)] public float dashInputBufferTime;

    private void OnValidate()
    {
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        gravityScale = gravityStrength / Physics.gravity.y;

        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }
}
