using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    // KnockbackForce�� ��Ȳ�� ���� ������.
    // HitType�� �������� �ʾҾ�.
    public void ApplyDamage(float damageAmount, Vector3 hitPoint, Vector3 hitDirection, float knockbackForce, Entity entity);
}
