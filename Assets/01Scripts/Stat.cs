using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class Stat
{
    [SerializeField] private float _baseValue;

    public List<float> modifiers = new List<float>(2);
    public bool isPercent;

    public float GetValue()
    {
        float resultValue = _baseValue;
        foreach (float value in modifiers)
        {
            resultValue += value;
        }
        return resultValue;
    }

    public void AddModifier(float value)
    {
        if(value != 0)
            modifiers.Add(value);
    }

    public void RemoveModifier(float value)
    {
        if(value != 0)
            modifiers.Remove(value);
    }

    public void SetDefaultValue(float value)
    {
        _baseValue = value;
    }
}
