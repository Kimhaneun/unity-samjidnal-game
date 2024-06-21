using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    private Entity _entity;
    private Collider[] _attackRangeColliders;

    // const로 바꾸어 보자
    private int _attackColliderCount = 1;

    [SerializeField] private Transform _attackDistanceCheckPoint;
    [SerializeField] public Vector3 attackDistance;

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _obstacleLayer;

    public void Initialize(Entity entity)
    {
        _entity = entity;
    }

    private void Awake()
    {
        _attackRangeColliders = new Collider[_attackColliderCount];
    }

    public bool IsCastDamage()
    {
        Collider collider = Test();
        if (collider != null)
        {
            if(collider.TryGetComponent<IDamageble>(out IDamageble health))
            {
                if (!IsObstacleInLine(attackDistance.magnitude, _attackDistanceCheckPoint.forward)) 
                { 
                    float damage = _entity.EntityStat.GetDamage();
              
                    health.ApplyDamage(damage, _attackDistanceCheckPoint.position, attackDistance * 0.5f, 3f, _entity);
                }
            }
        }
        return collider;
    }

    private Collider Test()
    {
        int cnt = Physics.OverlapBoxNonAlloc(_attackDistanceCheckPoint.position, attackDistance * 0.5f, _attackRangeColliders, quaternion.identity, _playerLayer);
        return cnt >= 1 ? _attackRangeColliders[0] : null;
    }

    private bool IsObstacleInLine(float distance, Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, distance, _obstacleLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_attackDistanceCheckPoint.position, attackDistance);
        Gizmos.color = Color.yellow;
    }
}