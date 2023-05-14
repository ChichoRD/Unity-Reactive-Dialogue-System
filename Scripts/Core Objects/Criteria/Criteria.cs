using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Criteria
{
    [field: SerializeReference] public List<ICriteriaCheckableCondition> Conditions { get; set; } 
    public int ConditionCount => Conditions.Count;

    public bool IsSatisfied() => Conditions.TrueForAll(c => c.IsMet());
    public bool IsSatisfied(out int amountMet)
    {
        amountMet = 0;
        foreach (var condition in Conditions)
        {
            if (!condition.IsMet()) continue;
            amountMet++;
        }

        return amountMet == ConditionCount;
    }
}
