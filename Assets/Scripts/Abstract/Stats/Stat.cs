using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat : MonoBehaviour
{
    private readonly List<int> modifiers = new List<int>();
    [SerializeField] private int baseValue;

    public int GetValue()
    {
        var finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }
}