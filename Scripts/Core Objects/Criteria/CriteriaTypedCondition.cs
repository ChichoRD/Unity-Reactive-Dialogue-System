using System;
using UnityEngine;

[Serializable]
public class CriteriaTypedCondition : ICriteriaCondition
{
    [field: SerializeField] public FactEntryObject Fact { get; private set; }
    [SerializeField] private PredicateType _predicateType;

    public Func<int, bool> Predicate()
    {
        return value => _predicateType switch
        {
            PredicateType.Equals => Fact.Value == value,
            PredicateType.NotEquals => Fact.Value != value,
            PredicateType.GreaterThan => Fact.Value > value,
            PredicateType.LessThan => Fact.Value < value,
            PredicateType.GreaterThanOrEquals => Fact.Value >= value,
            PredicateType.LessThanOrEquals => Fact.Value <= value,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [Serializable]
    private enum PredicateType
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEquals,
        LessThanOrEquals
    }
}