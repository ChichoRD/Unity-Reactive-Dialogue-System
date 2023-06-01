using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DialogueEventsNotifier : MonoBehaviour
{
    [SerializeField] private DialogueEventsListener[] _dialogueEventsListeners = new DialogueEventsListener[] { };
    private readonly HashSet<FactEntryObject> _observedFacts = new HashSet<FactEntryObject>();

    [field: SerializeField] public UnityEvent OnBecomeRaisable { get; private set; }
    [field: SerializeField] public UnityEvent OnBecomeUnraisable { get; private set; }

    private bool _hasRaisableListeners;
    public bool HasRaisableListeners
    {
        get => _hasRaisableListeners;
        set
        {
            bool hadRaisableListeners = _hasRaisableListeners;
            _hasRaisableListeners = value;

            if (hadRaisableListeners && !_hasRaisableListeners)
                OnBecomeUnraisable?.Invoke();
            if (!hadRaisableListeners && _hasRaisableListeners)
                OnBecomeRaisable?.Invoke();
        }
    }

    private void Awake()
    {
        FilterObservedFacts();

        HasRaisableListeners = GetRaisability();
    }

    private void OnEnable()
    {
        foreach (var fact in _observedFacts)
        {
            fact.OnValueChanged.RemoveListener(OnFactValueChanged);
            fact.OnValueChanged.AddListener(OnFactValueChanged);
        }
    }

    private void OnDisable()
    {
        foreach (var fact in _observedFacts)
            fact.OnValueChanged.RemoveListener(OnFactValueChanged);
    }

    private void FilterObservedFacts()
    {
        _observedFacts.Clear();

        foreach (var dialogueEventsListener in _dialogueEventsListeners)
            foreach (var eventEntry in dialogueEventsListener.EventsEntries)
                foreach (var rule in eventEntry.ListenerRules)
                    foreach (var condition in rule.Criteria.Conditions)
                        _observedFacts.Add(condition.Fact);
    }

    private bool GetRaisability() => _dialogueEventsListeners.Any(listener => listener.Raisable);
    private void OnFactValueChanged(int arg0) => HasRaisableListeners = GetRaisability();
}