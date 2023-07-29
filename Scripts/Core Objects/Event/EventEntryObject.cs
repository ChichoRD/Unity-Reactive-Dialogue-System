using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = NEW_WORD + OBJECT_NAME, menuName = PATH + OBJECT_NAME)]
public class EventEntryObject : DialogueEntryObject
{
    public const string OBJECT_NAME = "Event Entry";
    [field: SerializeField] public List<RuleEntryObject> ListenerRules { get; private set; }
    [field: SerializeField] public UnityEvent<RuleEntryObject> OnDispatched { get; private set; }

    [SerializeField][Range(0.0f, 1.0f)] private float _criteriaSatisfactionThreshold = 1.0f;
    [SerializeField] private bool _dispatchAllThatPass;

    [ContextMenu(nameof(Dispatch))]
    public void Dispatch()
    {
        if (_dispatchAllThatPass)
        {
            var rules = GetSuccessfullyDispatchingRules(ListenerRules);
            foreach (var rule in rules)
            {
                OnDispatched?.Invoke(rule);
                rule.OnDispatched?.Invoke();
            }

            return;
        }

        var bestRule = GetSuccessfullyDispatchingRules(ListenerRules).FirstOrDefault();
        if (bestRule == null) return;

        OnDispatched?.Invoke(bestRule);
        bestRule.OnDispatched?.Invoke();
    }

    public IEnumerable<RuleEntryObject> GetSuccessfullyDispatchingRules(IEnumerable<RuleEntryObject> rules)
    {
        var orderedRules = rules.Distinct().Where(r => r != null).OrderByDescending(r => r.Criteria.ConditionCount).ToList();
        return orderedRules.Where(r =>
        {
            bool satisfied = r.Criteria.IsSatisfied(out int amountMet);
            return satisfied || amountMet / r.Criteria.ConditionCount >= _criteriaSatisfactionThreshold;
        });
    }
}