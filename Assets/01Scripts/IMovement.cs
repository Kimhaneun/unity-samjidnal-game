using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    public bool IsJumpFalling { get; }
    public Vector3 Velocity { get; }
    public void Initialize(Entity entity);
    public void SetVelocity(float x, float y, float z, bool doNotTurn = false);
    public void SetDestination(Vector3 destination);
    public void StopImmediately(bool withAxisY, bool withAxisZ); 
    public bool DetectGround();
    public void SetJump();
}
