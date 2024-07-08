using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    public bool IsJumpFalling { get; }
    public Vector3 Velocity { get; }
    public void Initialize(Entity entity);
    // 원래는 Vector3로 받는데... 일단 이랗게 받아요.
    // 원래는 true입니다. Entity에 따라 false로 두었어요.
    public void SetVelocity(float x, float y, float z, bool doNotTurn = false);
    public void SetDestination(Vector3 destination);
    public void StopImmediately(bool withAxisY, bool withAxisZ); // 만약 구현부가 전부 같다면 그냥 어기서 내용을 선언하여볼까? 작동 여부는 모르겠다.
    public bool DetectGround();
    public void SetJump();
    // Knockback
}
