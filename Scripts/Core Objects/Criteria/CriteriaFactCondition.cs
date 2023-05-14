using System;
using UnityEngine;

[Serializable]
public class CriteriaFactCondition : ICriteriaCheckableCondition
{
    [SerializeField] private CriteriaTypedCondition _criteriaTypedCondition;
    [SerializeField] private FactEntryObject _otherFact;

    public FactEntryObject Fact => _criteriaTypedCondition.Fact;
    public bool IsMet() => Predicate()(_otherFact.Value);
    public Func<int, bool> Predicate() => _criteriaTypedCondition.Predicate();
}
