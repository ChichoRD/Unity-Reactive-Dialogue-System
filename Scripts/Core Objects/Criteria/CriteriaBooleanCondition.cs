using System;
using UnityEngine;

[Serializable]
public class CriteriaBooleanCondition : ICriteriaCheckableCondition
{
    [SerializeField] private FactEntryObject _fact;
    [SerializeField] private bool _value;

    public FactEntryObject Fact => _fact;
    public bool IsMet() => Predicate()(_fact.Value);
    public Func<int, bool> Predicate() => value => value == (_value ? 1 : 0);
}
