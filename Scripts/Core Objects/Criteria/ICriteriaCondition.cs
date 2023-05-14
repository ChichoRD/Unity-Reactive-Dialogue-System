using System;

public interface ICriteriaCondition
{
    FactEntryObject Fact { get; }
    Func<int, bool> Predicate();
}