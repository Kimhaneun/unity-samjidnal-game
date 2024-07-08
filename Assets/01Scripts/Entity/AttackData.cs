using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/AttackData")]
public class AttackData : ScriptableObject
{
    public float attackSpeed;
    public float counterAttackDuration;
    public Vector3[] attackMovement;
    [HideInInspector] public int currentComboCounter;
}
