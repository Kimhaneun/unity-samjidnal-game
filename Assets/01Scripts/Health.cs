using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour, IDamageble
{
    public UnityEvent OnHitEvent;
    public UnityEvent OnDeathEvent;

    public ImpactInfo impactInfo;

    private float _currentHealth;
    private Entity _entity;

    public void Initialize(Entity entity)
    {
        _entity = entity;
        impactInfo = new ImpactInfo();
        _currentHealth = _entity.EntityStat.maxHealth.GetValue();
    }


    public void ApplyDamage(float damageAmount, Vector3 hitPoint, Vector3 hitDirection, float knockbackForce, Entity entity)
    {
        if (_entity.IsDead)
            return;

        impactInfo.hitPoint = hitPoint;
        impactInfo.hitDirection = hitDirection;

        if(knockbackForce > Mathf.Epsilon)
        {
            // ApplyKnockback(Vector3 hitDirection, Vector3 constantForceDirection, float inputDirection);
        }

        damageAmount = _entity.EntityStat.GetDamage();
        _currentHealth = Mathf.Clamp(_currentHealth - damageAmount, 0, entity.EntityStat.maxHealth.GetValue());
        OnHitEvent?.Invoke();
        
        if(_currentHealth <= 0)
        {
            OnDeathEvent?.Invoke();
        }
    }
}
