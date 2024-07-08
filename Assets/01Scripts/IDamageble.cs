using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    // KnockbackForce는 상황에 따라 없엔다.
    // HitType을 선언하지 않았어.
    public void ApplyDamage(float damageAmount, Vector3 hitPoint, Vector3 hitDirection, float knockbackForce, Entity entity);
}
