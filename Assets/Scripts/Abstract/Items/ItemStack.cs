using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemStack : Item
{
    [SerializeField] protected int amount;

    public int GetAmount()
    {
        return amount;
    }

    public virtual void Add(int value)
    {
        amount += value;
    }

    public virtual void SetRandomAmount(int minValue, int maxValue)
    {
        amount = Random.Range(minValue, maxValue + 1);
    }

    public virtual void SetSpecificAmount(int specifiсValue)
    {
        amount = specifiсValue;
    }
}