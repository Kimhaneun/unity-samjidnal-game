using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(menuName = "SO/Stat")]
public class EntityStat : ScriptableObject
{
    public Stat damage;
    public Stat maxHealth;

    protected Entity _entity;
    protected Dictionary<StatTypeEnum, Stat> _statDictionary;

    public virtual void SetEntity(Entity entity)
    {
        _entity = entity;
    }

    public virtual void IncreaseStatFor(float modifyValue, float duration, Stat statToModify)
    {
        _entity.StartCoroutine(StatModifyCoroutine(modifyValue, duration, statToModify));
    }

    protected IEnumerator StatModifyCoroutine(float value, float duration, Stat targetStat)
    {
        targetStat.AddModifier(value);
        yield return new WaitForSeconds(duration);
        targetStat.RemoveModifier(value);
    }

    protected virtual void OnEnable()
    {
        _statDictionary = new Dictionary<StatTypeEnum, Stat>();

        Type entityStatType = typeof(EntityStat);

        foreach(StatTypeEnum statTypeEnum in Enum.GetValues(typeof(StatTypeEnum)))
        {
            try
            {
                string fieldName = LowerFirstChar(statTypeEnum.ToString());

                FieldInfo statField = entityStatType.GetField(fieldName);
                _statDictionary.Add(statTypeEnum, statField.GetValue(this) as Stat);
            }
            catch (Exception ex)
            {
                Debug.LogError($"There are no stat - {statTypeEnum.ToString()}, {ex.Message}");
            }
        }
    }

    private string LowerFirstChar(string input) => $"{char.ToLower(input[0])}{input.Substring(1)}";

    public float GetDamage()
    {
        return damage.GetValue();
    }

    public void AddModifier(StatTypeEnum type, float value)
    {
        _statDictionary[type].AddModifier(value);
    }

    public void RemoveModifier(StatTypeEnum type, float value)
    {
        _statDictionary[type].RemoveModifier(value);
    }
}
