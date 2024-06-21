using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    public bool IsJumpFalling { get; }
    public Vector3 Velocity { get; }
    public void Initialize(Entity entity);
    // ������ Vector3�� �޴µ�... �ϴ� �̶��� �޾ƿ�.
    // ������ true�Դϴ�. Entity�� ���� false�� �ξ����.
    public void SetVelocity(float x, float y, float z, bool doNotTurn = false);
    public void SetDestination(Vector3 destination);
    public void StopImmediately(bool withAxisY, bool withAxisZ); // ���� �����ΰ� ���� ���ٸ� �׳� ��⼭ ������ �����Ͽ�����? �۵� ���δ� �𸣰ڴ�.
    public bool DetectGround();
    public void SetJump();
    // Knockback
}
