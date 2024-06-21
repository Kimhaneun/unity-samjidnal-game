using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아 이름을 기본 공격 데이터 라고 해야 맞을려나?
[CreateAssetMenu(menuName = "SO/Entity/AttackData")]
public class AttackData : ScriptableObject
{
    public float attackSpeed;
    public float counterAttackDuration;
    public Vector3[] attackMovement;
    [HideInInspector] public int currentComboCounter;
}
