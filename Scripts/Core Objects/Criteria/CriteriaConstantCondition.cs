using System;
using UnityEngine;

[Serializable]
public class CriteriaConstantCondition : ICriteriaCheckableCondition
{
    [SerializeField] private CriteriaTypedCondition _criteriaTypedCondition;
    [SerializeField] private int _value;

    public FactEntryObject Fact => _criteriaTypedCondition.Fact;
    public bool IsMet() => Predicate()(_value);
    public Func<int, bool> Predicate() => _criteriaTypedCondition.Predicate();
}
